namespace DeviantArtFs

open System.Net
open FSharp.Control
open FSharp.Json

module internal Dafs =
    /// Converts a guid (and only a guid) to a string.
    let guid2str (g: System.Guid) = g.ToString()

    /// URL-encodes a string.
    let urlEncode = WebUtility.UrlEncode

    [<RequireQualifiedAccess>]
    type Method = GET | POST

    /// Creates a DeviantArtRequest object, given a method, token object, common parameters, a URL, and a query string.
    let createRequest method token url query =
        let q = String.concat "&" query
        match method with
        | Method.GET ->
            new DeviantArtRequest(token, sprintf "%s?%s" url q, Method = "GET")
        | Method.POST ->
            new DeviantArtRequest(token, url, Method = "POST", ContentType = "application/x-www-form-urlencoded", RequestBodyText = q)

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
            match resp.Cursor with
            | Some c -> cursor <- c
            | None -> has_more <- false
    }