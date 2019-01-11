﻿namespace DeviantArtFs.Requests.Gallery

open DeviantArtFs
open DeviantArtFs.Interop

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
            yield! paging.ToQuery()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/folders?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return dafs.parsePage FoldersElement.Parse json
    }

    let ToAsyncSeq token req offset = AsyncExecute token |> dafs.toAsyncSeq offset 50 req

    let ToListAsync token req ([<Optional; DefaultParameterValue(0)>] offset: int) ([<Optional; DefaultParameterValue(2147483647)>] limit: int) =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toListAsync
        |> iop.thenMap (fun f -> {
            new IDeviantArtFolder with
                member __.Folderid = f.Folderid
                member __.Parent = f.Parent |> Option.toNullable
                member __.Name = f.Name
                member __.Size = f.Size |> Option.toNullable
        })
        |> Async.StartAsTask

    let ExecuteAsync token req paging =
        AsyncExecute token req paging
        |> iop.thenMapResult (fun f -> {
            new IDeviantArtFolder with
                member __.Folderid = f.Folderid
                member __.Parent = f.Parent |> Option.toNullable
                member __.Name = f.Name
                member __.Size = f.Size |> Option.toNullable
        })
        |> Async.StartAsTask