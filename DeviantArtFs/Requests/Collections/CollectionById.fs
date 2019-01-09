namespace DeviantArtFs.Requests.Collections

open DeviantArtFs
open DeviantArtFs.Interop
open System

type CollectionByIdRequest(folderid: Guid) =
    member __.Folderid = folderid
    member val Username = null with get, set
    member val Offset = 0 with get, set
    member val Limit = 10 with get, set

module CollectionById =
    let AsyncExecute token (req: CollectionByIdRequest) = async {
        let query = seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (dafs.urlEncode s)
            | None -> ()
            yield sprintf "offset=%d" req.Offset
            yield sprintf "limit=%d" req.Limit
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/collections/%A?%s" req.Folderid
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return json |> dafs.parsePage DeviationResponse.Parse
    }

    let ExecuteAsync token id = AsyncExecute token id |> iop.thenMapResult Deviation |> Async.StartAsTask