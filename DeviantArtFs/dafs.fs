namespace DeviantArtFs

open System
open System.Net
open FSharp.Control
open FSharp.Json

module internal Dafs =
    /// URL-encodes a string.
    let urlEncode = WebUtility.UrlEncode

    /// Creates a DeviantArtRequest object, given a token object, common parameters, and a URL.
    let createRequest (token: IDeviantArtAccessToken) (url: string) =
        new DeviantArtRequest(token, url)

    /// Creates a DeviantArtRequest object, given a token object, common parameters, a URL, and a query string.
    let createRequest2 token url query =
        let q = String.concat "&" query
        let u = sprintf "%s?%s" url q
        createRequest token u

    /// Executes a DeviantArtRequest and gets the response body.
    let asyncRead (req: DeviantArtRequest) = req.AsyncReadJson()

    /// Parses a JSON string as the given type.
    let parse<'a> str = Json.deserialize<'a> str

    /// Takes an async workflow that returns a JSON string, and creates another async workflow that will deserialize it to the given type.
    let thenParse<'a> workflow =
        workflow
        |> AsyncThen.map parse<'a>

    /// Takes an async workflow, and waits for it but ignores its result.
    let thenIgnore (workflow: Async<'a>) =
        workflow
        |> AsyncThen.map ignore

    /// Takes an async workflow that returns a DeviantArtListOnlyResponse, and creates another async workflow that returns the list it contains.
    let extractList (workflow: Async<'a> when 'a :> IDeviantArtListOnlyResponse<'b>) =
        workflow
        |> AsyncThen.map (fun x -> x.List)

    /// Takes an async workflow that returns a DeviantArtTextOnlyResponse, and creates another async workflow that returns the text it contains.
    let extractText<'a> (workflow: Async<DeviantArtTextOnlyResponse>) =
        workflow
        |> AsyncThen.map (fun x -> x.text)

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

    let toAsyncSeq3 (cursor: 'a) (f: 'a -> Async<'b> when 'b :> IResultPage<'a, 'item>) = asyncSeq {
        let mutable cursor = cursor
        let mutable has_more = true
        while has_more do
            let! resp = f cursor
            for r in resp.Items do
                yield r
            cursor <- resp.Cursor
            has_more <- resp.HasMore
    }