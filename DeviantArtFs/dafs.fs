namespace DeviantArtFs

open System
open System.Net
open System.IO
open System.Threading.Tasks
open FSharp.Control

module internal Dafs =
    open System.Text

    let assertSuccess (resp: DeviantArtSuccessOrErrorResponse) =
        match (resp.success, resp.error_description) with
        | (true, None) -> ()
        | _ -> failwithf "%s" (resp.error_description |> Option.defaultValue "An unknown error occurred.")

    let urlEncode = WebUtility.UrlEncode

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
        new DeviantArtRequest(token, full_url)

    let asyncRead (req: DeviantArtRequest) = req.AsyncReadJson()

    let getMax (f: IDeviantArtAccessToken -> IDeviantArtPagingParams -> 'a) (token: IDeviantArtAccessToken) (offset: int) =
        new DeviantArtPagingParams(Offset = offset, Limit = Nullable Int32.MaxValue)
        |> f token

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