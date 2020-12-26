﻿namespace DeviantArtFs.Api.Stash

open DeviantArtFs

module Position =
    let AsyncExecute token common (stackid: int64) (position: int) = async {
        let query = seq {
            yield sprintf "position=%d" position
            yield! QueryFor.commonParams common
        }

        let req = Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/position/%d" stackid) Seq.empty
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        req.RequestBodyText <- String.concat "&" query

        return! req
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>
    }

    let ExecuteAsync token common stackid position =
        AsyncExecute token common stackid position
        |> Async.StartAsTask