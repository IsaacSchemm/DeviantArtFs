namespace DeviantArtFs.Api.Collections

open System
open DeviantArtFs

type FaveResponse = {
    success: bool
    favourites: int
}

module Fave =
    let AsyncExecute token (deviationid: Guid) (folderids: seq<Guid>) = async {
        let query = seq {
            yield sprintf "deviationid=%O" deviationid
            let mutable index = 0
            for f in folderids do
                yield sprintf "folderid[%d]=%O" index f
                index <- index + 1
        }

        let req = Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/collections/fave" Seq.empty
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        req.RequestBodyText <- String.concat "&" query

        return! req
        |> Dafs.asyncRead
        |> Dafs.thenParse<FaveResponse>
    }

    let ExecuteAsync token deviationid folderids =
        AsyncExecute token deviationid folderids
        |> Async.StartAsTask