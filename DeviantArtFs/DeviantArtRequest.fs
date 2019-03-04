namespace DeviantArtFs

open System.Net
open System.IO

type DeviantArtRequest(token: IDeviantArtAccessToken, url: string) =
    let is401 (response: WebResponse) =
        match response with
        | :? HttpWebResponse as h -> h.StatusCode = HttpStatusCode.Unauthorized
        | _ -> false

    static member UserAgent = "DeviantArtFs/1.1 (https://github.com/libertyernie/DeviantArtFs)"

    member val Method: string = "GET" with get, set
    member val ContentType: string = null with get, set
    member val RequestBody: byte[] = null with get, set

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
        return req
    }

    member this.AsyncGetResponse() = async {
        let! req = this.AsyncToWebRequest()
        try
            req.Headers.["Authorization"] <- sprintf "Bearer %s" token.AccessToken
            return! req.AsyncGetResponse()
        with
            | :? WebException as ex when (is401 ex.Response) && (token :? IDeviantArtAutomaticRefreshToken) ->
                let auto = token :?> IDeviantArtAutomaticRefreshToken
                let! newRefreshToken = auto.DeviantArtAuth.RefreshAsync auto.RefreshToken |> Async.AwaitTask
                do! auto.UpdateTokenAsync newRefreshToken |> Async.AwaitTask

                let! req2 = this.AsyncToWebRequest()
                req2.Headers.["Authorization"] <- sprintf "Bearer %s" auto.AccessToken
                return! req2.AsyncGetResponse()
    }