namespace DeviantArtFs

open System.Net
open System.IO
open System
open System.Text

type DeviantArtRequest(token: IDeviantArtAccessToken, url: string) =
    let isStatus (code: int) (response: WebResponse) =
        match response with
        | :? HttpWebResponse as h -> int h.StatusCode = code
        | _ -> false

    let mutable retry429 = 500

    static member UserAgent = "DeviantArtFs/1.1 (https://github.com/libertyernie/DeviantArtFs)"

    member val Method: string = "GET" with get, set
    member val ContentType: string = null with get, set
    member val RequestBody: byte[] = null with get, set

    member this.RequestBodyText
        with set(value: string) = this.RequestBody <- Encoding.UTF8.GetBytes value

    member private this.AsyncToWebRequest() = async {
        let req = WebRequest.CreateHttp url
        req.UserAgent <- DeviantArtRequest.UserAgent
        req.Method <- this.Method
        req.ContentType <- this.ContentType
        if not (isNull this.RequestBody) then
            do! async {
                use! stream = req.GetRequestStreamAsync() |> Async.AwaitTask
                use ms = new MemoryStream(this.RequestBody, false)
                do! ms.CopyToAsync(stream) |> Async.AwaitTask
            }
        req.Headers.["Authorization"] <- sprintf "Bearer %s" token.AccessToken
        return req
    }

    member this.AsyncGetResponse() = async {
        let! req = this.AsyncToWebRequest()
        try
            return! req.AsyncGetResponse()
        with
            | :? WebException as ex when isStatus 401 ex.Response && (token :? IDeviantArtAutomaticRefreshToken) ->
                let auto = token :?> IDeviantArtAutomaticRefreshToken
                let! newRefreshToken = auto.DeviantArtAuth.RefreshAsync auto.RefreshToken |> Async.AwaitTask
                do! auto.UpdateTokenAsync newRefreshToken |> Async.AwaitTask

                let! req2 = this.AsyncToWebRequest()
                return! req2.AsyncGetResponse()
            | :? WebException as ex when isStatus 429 ex.Response ->
                retry429 <- Math.Max(retry429 * 2, 60000)
                if retry429 >= 60000 then
                    return failwithf "Client is rate-limited (too many 429 responses)"
                do! Async.Sleep retry429

                let! req2 = this.AsyncToWebRequest()
                return! req2.AsyncGetResponse()
            | :? WebException as ex ->
                use resp = ex.Response
                use sr = new StreamReader(resp.GetResponseStream())
                let! json = sr.ReadToEndAsync() |> Async.AwaitTask
                let error_obj = DeviantArtBaseResponse.Parse json
                return raise (new DeviantArtException(resp, error_obj))
    }

    member this.AsyncReadJson() = async {
        use! resp = this.AsyncGetResponse()
        use sr = new StreamReader(resp.GetResponseStream())
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask
        let obj = DeviantArtBaseResponse.Parse json
        if obj.status = Some "error" then
            return raise (new DeviantArtException(resp, obj))
        else
            return json
    }