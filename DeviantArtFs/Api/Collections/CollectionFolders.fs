namespace DeviantArtFs.Api.Collections

open DeviantArtFs
open FSharp.Control

type CollectionFoldersRequest() =
    member val Username = null with get, set
    member val CalculateSize = false with get, set
    member val ExtPreload = false with get, set

module CollectionFolders =
    let AsyncExecute token (req: CollectionFoldersRequest) paging =
        seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (Dafs.urlEncode s)
            | None -> ()
            yield sprintf "calculate_size=%b" req.CalculateSize
            yield sprintf "ext_preload=%b" req.ExtPreload
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/collections/folders"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtCollectionFolder>>

    let ToAsyncSeq token req offset =
        Dafs.toAsyncSeq offset (AsyncExecute token req)

    let ToArrayAsync token req offset limit =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token req paging =
        AsyncExecute token req paging
        |> Async.StartAsTask