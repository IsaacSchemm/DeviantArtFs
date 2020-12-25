namespace DeviantArtFs.Api.Collections

open DeviantArtFs
open System

type CollectionByIdRequest(folderid: Guid) =
    member __.Folderid = folderid
    member val Username = null with get, set

module CollectionById =
    open FSharp.Control

    let AsyncExecute token common paging (req: CollectionByIdRequest) =
        seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 24
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token (sprintf "https://www.deviantart.com/api/v1/oauth2/collections/%A" req.Folderid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtFolderPagedResult>

    let ToAsyncSeq token common offset req =
        Dafs.toAsyncSeq3 offset (fun o -> AsyncExecute token common { Offset = o; Limit = DeviantArtPagingParams.Max } req)

    let ToArrayAsync token common offset limit req =
        ToAsyncSeq token common offset req
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token common paging req =
        AsyncExecute token common paging req
        |> Async.StartAsTask