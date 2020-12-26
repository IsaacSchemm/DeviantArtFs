namespace DeviantArtFs

open System.Net
open FSharp.Control
open FSharp.Json

module internal Dafs =
    /// URL-encodes a string.
    let urlEncode = WebUtility.UrlEncode

    /// Creates a DeviantArtRequest object, given a token object, common parameters, a URL, and a query string.
    let createRequest token url query =
        let q = String.concat "&" query
        let u = sprintf "%s?%s" url q
        new DeviantArtRequest(token, u)

    /// Executes a DeviantArtRequest and gets the response body.
    let asyncRead (req: DeviantArtRequest) = req.AsyncReadJson()

    /// Parses a JSON string as the given type.
    let parse<'a> str = Json.deserialize<'a> str

    /// Takes an async workflow that returns a JSON string, and creates another async workflow that will deserialize it to the given type.
    let thenParse<'a> workflow =
        workflow
        |> AsyncThen.map parse<'a>

    /// Builds an AsyncSeq from an initial cursor and a function that uses that cursor to generate an IResultPage with the same cursor type.
    let toAsyncSeq (cursor: 'a) (f: 'a -> Async<'b> when 'b :> IDeviantArtResultPage<'a, 'item>) = asyncSeq {
        let mutable cursor = cursor
        let mutable has_more = true
        while has_more do
            let! resp = f cursor
            for r in resp.Items do
                yield r
            cursor <- resp.Cursor
            has_more <- resp.HasMore
    }