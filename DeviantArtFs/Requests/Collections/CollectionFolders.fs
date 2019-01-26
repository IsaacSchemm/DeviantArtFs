namespace DeviantArtFs.Requests.Collections

open DeviantArtFs

type CollectionFoldersRequest() =
    member val Username = null with get, set
    member val CalculateSize = false with get, set
    member val ExtPreload = false with get, set

module CollectionFolders =
    open FSharp.Control

    let AsyncExecute token (paging: IPagingParams) (ps: CollectionFoldersRequest) = async {
        let query = seq {
            match Option.ofObj ps.Username with
            | Some s -> yield sprintf "username=%s" (dafs.urlEncode s)
            | None -> ()
            yield sprintf "calculate_size=%b" ps.CalculateSize
            yield sprintf "ext_preload=%b" ps.ExtPreload
            yield! queryFor.paging paging
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/collections/folders?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return DeviantArtPagedResult<DeviantArtCollectionFolder>.Parse json
    }

    let ToAsyncSeq token req offset = AsyncExecute token |> dafs.toAsyncSeq offset 50 req

    let ToArrayAsync token req offset limit =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.map (fun f -> f :> IBclDeviantArtCollectionFolder)
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token paging req =
        AsyncExecute token paging req
        |> AsyncThen.mapPagedResult (fun f -> f :> IBclDeviantArtCollectionFolder)
        |> Async.StartAsTask