namespace DeviantArtFs.Api.Collections

open DeviantArtFs
open FSharp.Control

type CollectionFoldersRequest() =
    member val Username = null with get, set
    member val CalculateSize = false with get, set
    member val ExtPreload = false with get, set

module CollectionFolders =
    let AsyncExecute token common (req: CollectionFoldersRequest) paging =
        seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (Dafs.urlEncode s)
            | None -> ()
            yield sprintf "calculate_size=%b" req.CalculateSize
            yield sprintf "ext_preload=%b" req.ExtPreload
            yield! QueryFor.paging paging 50
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token "https://www.deviantart.com/api/v1/oauth2/collections/folders"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtCollectionFolder>>

    let ToAsyncSeq token common req offset =
        Dafs.toAsyncSeq3 offset (AsyncExecute token common req)

    let ToArrayAsync token common req offset limit =
        ToAsyncSeq token common req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common req paging =
        AsyncExecute token common req paging
        |> Async.StartAsTask