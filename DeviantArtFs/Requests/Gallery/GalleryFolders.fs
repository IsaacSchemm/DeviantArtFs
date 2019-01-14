﻿namespace DeviantArtFs.Requests.Gallery

open DeviantArtFs

type GalleryFoldersRequest() =
    member val Username = null with get, set
    member val CalculateSize = false with get, set

module GalleryFolders =
    open System.Runtime.InteropServices
    open FSharp.Control

    let AsyncExecute token (paging: PagingParams) (req: GalleryFoldersRequest) = async {
        let query = seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (dafs.urlEncode s)
            | None -> ()
            yield sprintf "calculate_size=%b" req.CalculateSize
            yield! paging.GetQuery()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/folders?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return dafs.parsePage (GalleryFoldersElement.Parse >> DeviantArtGalleryFolder) json
    }

    let ToAsyncSeq token req offset = AsyncExecute token |> dafs.toAsyncSeq offset 50 req

    let ToListAsync token req ([<Optional; DefaultParameterValue(0)>] offset: int) ([<Optional; DefaultParameterValue(2147483647)>] limit: int) =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toListAsync
        |> iop.thenMap (fun f -> f :> IBclDeviantArtGalleryFolder)
        |> Async.StartAsTask

    let ExecuteAsync token req paging =
        AsyncExecute token req paging
        |> iop.thenMapResult (fun f -> f :> IBclDeviantArtGalleryFolder)
        |> Async.StartAsTask