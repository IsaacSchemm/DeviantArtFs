namespace DeviantArtFs

open System
open System.Net
open FSharp.Control

module internal Dafs =
    /// URL-encodes a string.
    let urlEncode = WebUtility.UrlEncode

    /// Adds common parameters to a URL.
    let buildUrl (p: DeviantArtCommonParams) (url: string) =
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

    /// Creates a DeviantArtRequest object, given a token object, common parameters, and a URL.
    let createRequest (token: IDeviantArtAccessToken) (p: DeviantArtCommonParams) (url: string) =
        new DeviantArtRequest(token, buildUrl p url)

    /// Executes a DeviantArtRequest and gets the response body.
    let asyncRead (req: DeviantArtRequest) = req.AsyncReadJson()
    
    /// Converts a paged function with offset and limit parameters to one that requests the maximum page size each time.
    let getMax (f: IDeviantArtPagingParams -> 'a) (offset: int) =
        new DeviantArtPagingParams(Offset = offset, Limit = Nullable Int32.MaxValue)
        |> f

    /// Converts a paged function that takes a "cursor" as one of its parameters into an AsyncSeq.
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