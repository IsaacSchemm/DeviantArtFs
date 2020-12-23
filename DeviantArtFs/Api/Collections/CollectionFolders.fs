namespace DeviantArtFs.Api.Collections

open DeviantArtFs

type CollectionFoldersRequest() =
    member val Username = null with get, set
    member val CalculateSize = false with get, set
    member val ExtPreload = false with get, set

module CollectionFolders =
    open FSharp.Control

    let AsyncExecute token paging (ps: CollectionFoldersRequest) = async {
        let query = seq {
            match Option.ofObj ps.Username with
            | Some s -> yield sprintf "username=%s" (Dafs.urlEncode s)
            | None -> ()
            yield sprintf "calculate_size=%b" ps.CalculateSize
            yield sprintf "ext_preload=%b" ps.ExtPreload
            yield! QueryFor.paging paging 50
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/collections/folders?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtPagedResult<DeviantArtCollectionFolder>.Parse json
    }

    let ToAsyncSeq token offset req =
        Dafs.getMax (AsyncExecute token)
        |> Dafs.toAsyncSeq offset req

    let ToArrayAsync token offset limit req =
        ToAsyncSeq token offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> Async.StartAsTask