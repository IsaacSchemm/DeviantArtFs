namespace DeviantArtFs

open System
open System.Net.Http
open System.Threading
open FSharp.Control
open FSharp.Json
open DeviantArtFs.ResponseTypes

module internal Utils =
    type ImmutableRequest = {
        Method: HttpMethod
        Token: IDeviantArtAccessToken
        Url: string
        Content: HttpContent
    }

    [<RequireQualifiedAccess>]
    type Method = GET | POST

    let get token url query = {
        Method = HttpMethod.Get
        Token = token
        Url = $"{url}?{DeviantArtHttp.createQueryString query}"
        Content = null
    }

    let post token url query = {
        Method = HttpMethod.Post
        Token = token
        Url = url
        Content = DeviantArtHttp.createForm query
    }

    let mutable private retry429 = 500

    module internal RefreshLock =
        let Semaphore = new SemaphoreSlim(1, 1)

    let rec private getResponseAsync (ir: ImmutableRequest) = task {
        let accessToken = ir.Token.AccessToken
        use reqMessage = new HttpRequestMessage(ir.Method, ir.Url)
        reqMessage.Content <- ir.Content
        reqMessage.Headers.Add("Authorization", $"Bearer {accessToken}")
        let! respMessage = DeviantArtHttp.HttpClient.SendAsync(reqMessage)
        if int respMessage.StatusCode = 429 then
            retry429 <- Math.Max(retry429 * 2, 30000)
            if retry429 >= 30000 then
                return failwithf "Client is rate-limited (too many 429 responses)"
            else
                do! Async.Sleep retry429
                return! getResponseAsync ir
        else if not respMessage.IsSuccessStatusCode && ["application/json"; "text/json"] |> List.contains respMessage.Content.Headers.ContentType.MediaType then
            let! json = respMessage.Content.ReadAsStringAsync()
            let obj = Json.deserialize<BaseResponse> json

            match (ir.Token, obj.error) with
            | (:? IDeviantArtRefreshableAccessToken as fetcher, Some "invalid_token") ->
                do! RefreshLock.Semaphore.WaitAsync()
                try
                    if accessToken = ir.Token.AccessToken then
                        do! fetcher.RefreshAccessTokenAsync()
                finally
                    RefreshLock.Semaphore.Release() |> ignore
                return! getResponseAsync { ir with Token = { new IDeviantArtAccessToken with member __.AccessToken = fetcher.AccessToken } }
            | _ ->
                return raise (new DeviantArtException(respMessage, obj, json))
        else
            ignore (respMessage.EnsureSuccessStatusCode())
            return respMessage
    }

    let readAsync (req: ImmutableRequest) = task {
        use! resp = getResponseAsync req
        let! json = resp.Content.ReadAsStringAsync()
        let obj = Json.deserialize<BaseResponse> json
        if obj.status = Some "error" then
            return raise (new DeviantArtException(resp, obj, json))
        else
            return json
    }

    let intString (i: int) = string i

    let guidString (g: Guid) = string g

    let parse<'a> str = Json.deserialize<'a> str

    let thenMap f a = task {
        let! o = a
        return f o
    }

    let thenParse<'a> t =
        t |> thenMap parse<'a>
