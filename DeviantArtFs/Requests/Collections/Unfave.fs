namespace DeviantArtFs.Requests.Collections

open System
open System.IO
open DeviantArtFs

type internal UnfaveResponse = {
    success: bool
    favourites: int
}

module Unfave =
    open FSharp.Json

    let AsyncExecute token  (deviationid: Guid) (folderids: seq<Guid>) = async {
        let query = seq {
            yield sprintf "deviationid=%O" deviationid
            let mutable index = 0
            for f in folderids do
                yield sprintf "folderid[%d]=%O" index f
                index <- index + 1
        }

        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/collections/unfave"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        do! async {
            use! stream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(stream)
            do! String.concat "&" query |> sw.WriteAsync |> Async.AwaitTask
        }

        let! json = dafs.asyncRead req
        let resp = SuccessOrErrorResponse.Parse json
        dafs.assertSuccess resp

        let o = Json.deserialize<UnfaveResponse> json
        return o.favourites
    }

    let ExecuteAsync token deviationid folderids = AsyncExecute token deviationid folderids |> Async.StartAsTask