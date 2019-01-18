namespace DeviantArtFs.Requests.Collections

open DeviantArtFs

type CollectionFoldersRequest() =
    member val Username = null with get, set
    member val CalculateSize = false with get, set

module CollectionFolders =
    open System.Runtime.InteropServices
    open FSharp.Control

    let AsyncExecute token (paging: PagingParams) (ps: CollectionFoldersRequest) = async {
        let query = seq {
            match Option.ofObj ps.Username with
            | Some s -> yield sprintf "username=%s" (dafs.urlEncode s)
            | None -> ()
            yield sprintf "calculate_size=%b" ps.CalculateSize
            yield! paging.GetQuery()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/collections/folders?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return dafs.parsePage DeviantArtCollectionFolder.Parse json
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
        |> iop.thenMapResult (fun f -> f :> IBclDeviantArtCollectionFolder)
        |> Async.StartAsTask