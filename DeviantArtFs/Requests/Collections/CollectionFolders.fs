namespace DeviantArtFs.Requests.Collections

open DeviantArtFs
open DeviantArtFs.Interop

type CollectionFoldersRequest() =
    member val Username = null with get, set
    member val CalculateSize = false with get, set

module CollectionFolders =
    open System.Runtime.InteropServices
    open FSharp.Control

    let AsyncExecute token (ps: CollectionFoldersRequest) (paging: PagingParams) = async {
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
        return dafs.parsePage FoldersElement.Parse json
    }

    let ToAsyncSeq token req offset = AsyncExecute token req |> dafs.toAsyncSeq offset

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

    let ExecuteAsync token ps paging =
        AsyncExecute token ps paging
        |> iop.thenMapResult (fun f -> {
            new IDeviantArtFolder with
                member __.Folderid = f.Folderid
                member __.Parent = f.Parent |> Option.toNullable
                member __.Name = f.Name
                member __.Size = f.Size |> Option.toNullable
        })
        |> Async.StartAsTask