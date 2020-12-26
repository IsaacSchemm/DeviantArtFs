namespace DeviantArtFs.Api.Collections

open System
open DeviantArtFs

type FaveResponse = {
    success: bool
    favourites: int
}

module Fave =
    let AsyncExecute token (deviationid: Guid) (folderids: seq<Guid>) =
        seq {
            yield sprintf "deviationid=%O" deviationid
            let mutable index = 0
            for f in folderids do
                yield sprintf "folderid[%d]=%O" index f
                index <- index + 1
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/collections/fave"
        |> Dafs.asyncRead
        |> Dafs.thenParse<FaveResponse>

    let ExecuteAsync token deviationid folderids =
        AsyncExecute token deviationid folderids
        |> Async.StartAsTask