namespace DeviantArtFs.Requests.Collections

open DeviantArtFs
open System

type CollectionByIdRequest(folderid: Guid) =
    member __.Folderid = folderid
    member val Username = null with get, set

module CollectionById =
    open System.Runtime.InteropServices
    open FSharp.Control

    let AsyncExecute token (paging: PagingParams) (req: CollectionByIdRequest) = async {
        let query = seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (dafs.urlEncode s)
            | None -> ()
            yield! paging.GetQuery()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/collections/%A?%s" req.Folderid
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return dafs.parsePage (DeviationResponse.Parse >> Deviation) json
    }

    let ToAsyncSeq token req offset = AsyncExecute token |> dafs.toAsyncSeq offset 24 req

    let ToListAsync token req ([<Optional; DefaultParameterValue(0)>] offset: int) ([<Optional; DefaultParameterValue(2147483647)>] limit: int) =
        ToAsyncSeq token req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toListAsync
        |> iop.thenMap dafs.asBclDeviation
        |> Async.StartAsTask

    let ExecuteAsync token req paging = AsyncExecute token req paging |> iop.thenMapResult dafs.asBclDeviation |> Async.StartAsTask
