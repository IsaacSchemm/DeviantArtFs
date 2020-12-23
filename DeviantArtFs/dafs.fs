namespace DeviantArtFs

open System
open System.Net
open FSharp.Control

module internal Dafs =
    /// URL-encodes a string.
    let urlEncode = WebUtility.UrlEncode

    /// Creates a DeviantArtRequest object, given a token object, common parameters, and a URL.
    let createRequest (token: IDeviantArtAccessToken) (url: string) =
        new DeviantArtRequest(token, url)

    /// Executes a DeviantArtRequest and gets the response body.
    let asyncRead (req: DeviantArtRequest) = req.AsyncReadJson()
    
    /// Converts a paged function with offset and limit parameters to one that requests the maximum page size each time.
    let getMax (f: DeviantArtPagingParams -> 'a) (offset: int) =
        f ({ Offset = offset; Limit = Nullable Int32.MaxValue })

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

    let toAsyncSeq2 (offset: int) (f: DeviantArtPagingParams -> Async<'b> when 'b :> IResultPage<int, 'item>) = asyncSeq {
        let mutable cursor = offset
        let mutable has_more = true
        while has_more do
            let paging = { Offset = cursor; Limit = Nullable Int32.MaxValue }
            let! resp = f paging
            for r in resp.Items do
                yield r
            cursor <- resp.Cursor
            has_more <- resp.HasMore
    }