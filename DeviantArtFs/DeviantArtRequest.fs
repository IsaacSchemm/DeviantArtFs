﻿namespace DeviantArtFs

open System.Net
open System.IO
open System
open System.Text

type internal DeviantArtRequest(initial_token: IDeviantArtAccessToken, url: string) =
    let isStatus (code: int) (response: WebResponse) =
        match response with
        | :? HttpWebResponse as h -> int h.StatusCode = code
        | _ -> false

    let isJson (response: WebResponse) =
        if isNull response || isNull response.ContentType then
            false
        else
            let first = response.ContentType.Split(';') |> Seq.head
            ["application/json"; "text/json"] |> Seq.contains first

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
        return req
    }

    member private this.AsyncGetResponse (token: IDeviantArtAccessToken) = async {
        try
            let! req = this.AsyncToWebRequest()
            req.Headers.["Authorization"] <- sprintf "Bearer %s" token.AccessToken
            return! req.AsyncGetResponse()
        with
            | :? WebException as ex when isStatus 429 ex.Response ->
                retry429 <- Math.Max(retry429 * 2, 30000)
                if retry429 >= 30000 then
                    return failwithf "Client is rate-limited (too many 429 responses)"
                do! Async.Sleep retry429
                return! this.AsyncGetResponse token
            | :? WebException as ex when isJson ex.Response ->
                use resp = ex.Response
                use sr = new StreamReader(resp.GetResponseStream())
                let! json = sr.ReadToEndAsync() |> Async.AwaitTask
                let obj = DeviantArtBaseResponse.Parse json

                match (token, obj.error) with
                | (:? IDeviantArtAutomaticRefreshToken as auto, Some "invalid_token") ->
                    let! newToken = auto.DeviantArtAuth.RefreshAsync auto.RefreshToken |> Async.AwaitTask
                    do! auto.UpdateTokenAsync newToken |> Async.AwaitTask
                    return! this.AsyncGetResponse newToken
                | _ ->
                    return raise (new DeviantArtException(resp, obj))
    }

    member this.AsyncReadJson() = async {
        use! resp = this.AsyncGetResponse initial_token
        use sr = new StreamReader(resp.GetResponseStream())
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask
        let obj = DeviantArtBaseResponse.Parse json
        if obj.status = Some "error" then
            return raise (new DeviantArtException(resp, obj))
        else
            return json
    }