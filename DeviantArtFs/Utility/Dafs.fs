namespace DeviantArtFs

open System.Threading.Tasks
open System.Collections.Generic
open FSharp.Json
open DeviantArtFs.Pages

module internal Dafs =
    [<RequireQualifiedAccess>]
    type Method = GET | POST

    /// Converts a guid (and only a guid) to a string.
    let guid2str (g: System.Guid) = g.ToString()

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

    /// Takes an async workflow, and returns another that executes it and then applies the given function to the result
    let thenMap f a = async {
        let! o = a
        return f o
    }

    /// Takes an async workflow that returns a JSON string, and creates another async workflow that will deserialize it to the given type.
    let thenParse<'a> workflow =
        workflow
        |> thenMap parse<'a>

    /// Build an IAsyncEnumerable from an initial cursor and a function that uses that cursor to generate an IResultPage with the same cursor type.
    let toAsyncEnum (cursor: 'a) (f: 'a -> Async<'b> when 'b :> IPage<'a, 'item>) = {
        new IAsyncEnumerable<'item> with
            member __.GetAsyncEnumerator (token: System.Threading.CancellationToken) =
                let mutable buffer = []
                let mutable cursor = Some cursor
                let reload = async {
                    match (buffer, cursor) with
                    | ([], Some c) ->
                        let! new_page = f c
                        buffer <- new_page.Items
                        cursor <- new_page.NextPage
                    | _ -> ()
                }
                let mutable current = Unchecked.defaultof<_>
                {
                    new IAsyncEnumerator<'item> with
                        member __.Current = current
                        member __.MoveNextAsync() =
                            let moveNextAsync = async {
                                if List.isEmpty buffer then
                                    do! reload
                                match buffer with
                                | head::tail ->
                                    current <- head
                                    buffer <- tail
                                    return true
                                | [] ->
                                    return false
                            }

                            Async.StartAsTask (moveNextAsync, cancellationToken = token) |> ValueTask<bool>
                        member __.DisposeAsync() =
                            ValueTask ()
                }
    }