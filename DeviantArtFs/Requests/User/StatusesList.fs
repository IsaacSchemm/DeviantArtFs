﻿namespace DeviantArtFs.Requests.User

open DeviantArtFs
open DeviantArtFs.Interop
open FSharp.Data

type StatusesListRequest(username: string) =
    member __.Username = username
    member val Offset = 0 with get, set
    member val Limit = 10 with get, set

module StatusesList =
    let AsyncExecute token (req: StatusesListRequest) = async {
        let query = seq {
            yield sprintf "username=%s" (dafs.urlEncode req.Username)
            yield sprintf "offset=%d" req.Offset
            yield sprintf "limit=%d" req.Limit
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/statuses?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return dafs.parseGenericList StatusResponse.Parse json
    }

    let ExecuteAsync token req = AsyncExecute token req |> iop.thenMapResult Status |> Async.StartAsTask