namespace DeviantArtFs

open System
open System.Net
open System.IO
open System.Threading.Tasks
open FSharp.Control

module internal Dafs =
    let assertSuccess (resp: DeviantArtSuccessOrErrorResponse) =
        match (resp.success, resp.error_description) with
        | (true, None) -> ()
        | _ -> failwithf "%s" (resp.error_description |> Option.defaultValue "An unknown error occurred.")

    let urlEncode = WebUtility.UrlEncode
    let userAgent = "DeviantArtFs/0.6 (https://github.com/libertyernie/DeviantArtFs)"

    let createRequest (token: IDeviantArtAccessToken) (url: string) =
        let full_url =
            match token with
            | :? IDeviantArtAccessTokenWithCommonParameters as p ->
                let expand = seq {
                    if p.Expand.HasFlag(DeviantArtObjectExpansion.UserDetails) then
                        yield sprintf "user.details"
                    if p.Expand.HasFlag(DeviantArtObjectExpansion.UserGeo) then
                        yield sprintf "user.geo"
                    if p.Expand.HasFlag(DeviantArtObjectExpansion.UserProfile) then
                        yield sprintf "user.profile"
                    if p.Expand.HasFlag(DeviantArtObjectExpansion.UserStats) then
                        yield sprintf "user.stats"
                    if p.Expand.HasFlag(DeviantArtObjectExpansion.UserWatch) then
                        yield sprintf "user.watch"
                }
                let query = seq {
                    yield sprintf "mature_content=%b" p.MatureContent
                    if p.Expand <> DeviantArtObjectExpansion.None then
                        yield expand |> String.concat "," |> sprintf "expand=%s"
                }
                String.concat "" (seq {
                    yield url
                    if not (url.Contains("?")) then
                        yield "?"
                    yield query |> String.concat "&"
                })
            | _ -> url
        let req = WebRequest.CreateHttp full_url
        req.UserAgent <- userAgent
        req.Headers.["Authorization"] <- sprintf "Bearer %s" token.AccessToken
        req

    let mutable retry429 = 500

    let rec asyncRead (req: WebRequest) = async {
        try
            use! resp = req.AsyncGetResponse()
            use sr = new StreamReader(resp.GetResponseStream())
            let! json = sr.ReadToEndAsync() |> Async.AwaitTask
            let obj = DeviantArtBaseResponse.Parse json
            if obj.status = Some "error" then
                return raise (new DeviantArtException(resp, obj))
            else
                retry429 <- 500
                return json
        with
            | :? WebException as ex ->
                use resp = ex.Response :?> HttpWebResponse
                if int resp.StatusCode = 429 then
                    retry429 <- Math.Max(retry429 * 2, 60000)
                    if retry429 >= 60000 then
                        return failwithf "Client is rate-limited (too many 429 responses)"
                    do! Async.Sleep retry429
                    return! asyncRead req
                else
                    use sr = new StreamReader(resp.GetResponseStream())
                    let! json = sr.ReadToEndAsync() |> Async.AwaitTask
                    let error_obj = DeviantArtBaseResponse.Parse json
                    return raise (new DeviantArtException(resp, error_obj))
    }

    let page offset limit = new DeviantArtPagingParams(Offset = offset, Limit = Nullable limit)

    let toAsyncSeq (initial_cursor: 'cursor) (req: 'req) (f: 'cursor -> 'req -> Async<'b> when 'b :> IResultPage<'cursor, 'item>) = asyncSeq {
        let mutable cursor = initial_cursor
        let mutable has_more = true
        while has_more do
            let! resp = f cursor req
            for r in resp.Items do
                yield r
            cursor <- resp.Cursor
            has_more <- resp.HasMore
    }