namespace DeviantArtFs

open System
open System.Collections.Generic
open System.Net.Http
open System.Threading
open System.Threading.Tasks
open FSharp.Control
open FSharp.Json
open DeviantArtFs.ParameterTypes
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

    let private tokenQuery (t: IDeviantArtAccessToken) =
        match t with
        | :? IDeviantArtAccessTokenWithOptionalParameters as x -> QueryFor.optionalParameters x.OptionalParameters
        | _ -> Seq.empty

    let get token url query = {
        Method = HttpMethod.Get
        Token = token
        Url = $"{url}?{DeviantArtHttp.createQueryString query}&{DeviantArtHttp.createQueryString (tokenQuery token)}"
        Content = null
    }

    let postContent token url content = {
        Method = HttpMethod.Post
        Token = token
        Url = $"{url}?{DeviantArtHttp.createQueryString (tokenQuery token)}"
        Content = content
    }

    let post token url query =
        postContent token url (DeviantArtHttp.createForm query)

    module internal RefreshLock =
        let Semaphore = new SemaphoreSlim(1, 1)

    type internal RefreshMode = TokenRefreshable | TokenNonRefreshable

    let mutable MaximumDelayBetween429Retries = TimeSpan.FromSeconds(30.0)

    let rec private asyncGetResponse (refreshMode: RefreshMode) (immutableReq: ImmutableRequest) (retry429: int) = async {
        let accessToken = immutableReq.Token.AccessToken

        let! cancellationToken = Async.CancellationToken

        use reqMessage = new HttpRequestMessage(
            immutableReq.Method,
            immutableReq.Url)
        reqMessage.Content <- immutableReq.Content
        reqMessage.Headers.Add("Authorization", $"Bearer {accessToken}")

        let! respMessage =
            DeviantArtHttp.HttpClient.SendAsync(reqMessage, cancellationToken)
            |> Async.AwaitTask

        if int respMessage.StatusCode = 429 then
            if float retry429 >= MaximumDelayBetween429Retries.TotalSeconds then
                return failwithf "Client is rate-limited (too many 429 responses)"
            else
                do! Async.Sleep retry429
                return! asyncGetResponse refreshMode immutableReq (retry429 * 2)

        else if not respMessage.IsSuccessStatusCode && ["application/json"; "text/json"] |> List.contains respMessage.Content.Headers.ContentType.MediaType then
            let! json =
                respMessage.Content.ReadAsStringAsync()
                |> Async.AwaitTask
            let obj = Json.deserialize<BaseResponse> json

            match (immutableReq.Token, obj.error) with
            | (:? IDeviantArtRefreshableAccessToken as fetcher, Some "invalid_token") when refreshMode = TokenRefreshable ->
                do! RefreshLock.Semaphore.WaitAsync() |> Async.AwaitTask

                try
                    if accessToken = immutableReq.Token.AccessToken then
                        do! fetcher.RefreshAccessTokenAsync(cancellationToken) |> Async.AwaitTask
                finally
                    RefreshLock.Semaphore.Release() |> ignore

                return! asyncGetResponse TokenNonRefreshable immutableReq retry429
            | _ ->
                return raise (new DeviantArtException(respMessage, obj, json))

        else
            ignore (respMessage.EnsureSuccessStatusCode())
            return respMessage
    }

    let asyncRead (req: ImmutableRequest) = async {
        use! resp = asyncGetResponse TokenRefreshable req 500
        let! json = resp.Content.ReadAsStringAsync() |> Async.AwaitTask
        let obj = Json.deserialize<BaseResponse> json
        if obj.status = Some "error" then
            return raise (new DeviantArtException(resp, obj, json))
        else
            return json
    }

    let intString (i: int) = string i

    let guidString (g: Guid) = string g

    let parse<'a> str = Json.deserialize<'a> str

    let thenMap f a = async {
        let! o = a
        return f o
    }

    let thenParse<'a> t =
        t |> thenMap parse<'a>

    type AsyncSeqBuilderParameters<'TPage, 'TItem, 'TOffset> = {
        initial_offset: 'TOffset
        get_page: 'TOffset -> Async<'TPage>
        extract_data: 'TPage -> seq<'TItem>
        has_more: 'TPage -> bool
        extract_next_offset: 'TPage -> 'TOffset
    }

    let buildAsyncSeq (parameters: AsyncSeqBuilderParameters<'TPage, 'TItem, 'TOffset>) = asyncSeq {
        let mutable finished = false
        let mutable cursor = parameters.initial_offset
        while not finished do
            let! page = parameters.get_page cursor
            yield! parameters.extract_data page
            if parameters.has_more page then
                cursor <- parameters.extract_next_offset page
            else
                finished <- true
    }
