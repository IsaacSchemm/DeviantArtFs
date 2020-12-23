namespace DeviantArtFs.Api.Collections

open System
open System.IO
open DeviantArtFs

type FaveResponse = {
    success: bool
    favourites: int
}

module Fave =
    open FSharp.Json

    let AsyncExecute token (deviationid: Guid) (folderids: seq<Guid>) = async {
        let query = seq {
            yield sprintf "deviationid=%O" deviationid
            let mutable index = 0
            for f in folderids do
                yield sprintf "folderid[%d]=%O" index f
                index <- index + 1
        }

        let req = Dafs.createRequest token DeviantArtCommonParams.Default "https://www.deviantart.com/api/v1/oauth2/collections/fave"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        req.RequestBodyText <- String.concat "&" query

        let! json = Dafs.asyncRead req
        let o = Json.deserialize<FaveResponse> json
        return o.favourites
    }

    let ExecuteAsync token deviationid folderids = AsyncExecute token deviationid folderids |> Async.StartAsTask