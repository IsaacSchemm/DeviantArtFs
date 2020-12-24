namespace DeviantArtFs.Api.Collections

open DeviantArtFs
open FSharp.Control

type CollectionFoldersRequest() =
    member val Username = null with get, set
    member val CalculateSize = false with get, set
    member val ExtPreload = false with get, set

module CollectionFolders =
    let AsyncExecute token common paging (req: CollectionFoldersRequest) =
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

    let ToAsyncSeq token common offset req =
        (fun p -> AsyncExecute token common p req)
        |> Dafs.toAsyncSeq2 offset

    let ToArrayAsync token common offset limit req =
        ToAsyncSeq token common offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common paging req =
        AsyncExecute token common paging req
        |> Async.StartAsTask