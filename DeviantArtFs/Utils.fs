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

    let mutable private retry429 = 500

    module internal RefreshLock =
        let Semaphore = new SemaphoreSlim(1, 1)

    type internal RefreshMode = TokenRefreshable | TokenNonRefreshable

    let rec private getResponseAsync (refreshMode: RefreshMode) (ir: ImmutableRequest) = task {
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
                return! getResponseAsync refreshMode ir
        else if not respMessage.IsSuccessStatusCode && ["application/json"; "text/json"] |> List.contains respMessage.Content.Headers.ContentType.MediaType then
            let! json = respMessage.Content.ReadAsStringAsync()
            let obj = Json.deserialize<BaseResponse> json

            match (ir.Token, obj.error) with
            | (:? IDeviantArtRefreshableAccessToken as fetcher, Some "invalid_token") when refreshMode = TokenRefreshable ->
                do! RefreshLock.Semaphore.WaitAsync()
                try
                    if accessToken = ir.Token.AccessToken then
                        do! fetcher.RefreshAccessTokenAsync()
                finally
                    RefreshLock.Semaphore.Release() |> ignore
                return! getResponseAsync TokenNonRefreshable ir
            | _ ->
                return raise (new DeviantArtException(respMessage, obj, json))
        else
            ignore (respMessage.EnsureSuccessStatusCode())
            return respMessage
    }

    let readAsync (req: ImmutableRequest) = task {
        use! resp = getResponseAsync TokenRefreshable req
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

    type AsyncSeqBuilderParameters<'TPage, 'TItem, 'TOffset> = {
        initial_offset: 'TOffset
        get_page: 'TOffset -> System.Threading.Tasks.Task<'TPage>
        extract_data: 'TPage -> seq<'TItem>
        has_more: 'TPage -> bool
        extract_next_offset: 'TPage -> 'TOffset
    }

    let buildAsyncSeq (parameters: AsyncSeqBuilderParameters<'TPage, 'TItem, 'TOffset>) = {
        new IAsyncEnumerable<'TItem> with
            member _.GetAsyncEnumerator (token: System.Threading.CancellationToken) =
                let mutable buffer = []
                let mutable offset = parameters.initial_offset
                let mutable finished = false
                let mutable current = Unchecked.defaultof<_>
                {
                    new IAsyncEnumerator<'TItem> with
                        member _.Current = current
                        member _.MoveNextAsync() =
                            let workflow = async {
                                if List.isEmpty buffer && not finished then
                                    let! page = parameters.get_page offset |> Async.AwaitTask
                                    buffer <- parameters.extract_data page |> List.ofSeq
                                    if parameters.has_more page then
                                        offset <- parameters.extract_next_offset page
                                    else
                                        finished <- true
                                match buffer with
                                | head::tail ->
                                    current <- head
                                    buffer <- tail
                                    return true
                                | [] ->
                                    return false
                            }

                            Async.StartAsTask(workflow, cancellationToken = token) |> ValueTask<bool>
                        member _.DisposeAsync() =
                            new ValueTask()
                }
    }