namespace DeviantArtFs.Api.Collections

open DeviantArtFs
open System

type CollectionByIdRequest(folderid: Guid) =
    member __.Folderid = folderid
    member val Username = null with get, set

module CollectionById =
    open FSharp.Control

    let AsyncExecute token expansion (req: CollectionByIdRequest) paging =
        seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 24
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/collections/%A" req.Folderid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtFolderPagedResult>

    let ToAsyncSeq token expansion req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token expansion req)

    let ToArrayAsync token expansion req offset limit =
        ToAsyncSeq token expansion req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token expansion req paging =
        AsyncExecute token expansion req paging
        |> Async.StartAsTask