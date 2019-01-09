﻿namespace DeviantArtFs.Requests.Collections

open System
open System.IO
open DeviantArtFs
open FSharp.Data

type internal FaveResponse = JsonProvider<"""{
    "success": true,
    "favourites": 2
}""">

type FaveRequest(deviationid: Guid) =
    member __.Deviationid = deviationid
    member val Folderids = Seq.empty<Guid> with get, set

module Fave =
    let AsyncExecute token (req: FaveRequest) = async {
        let query = seq {
            yield sprintf "deviationid=%O" req.Deviationid
            let mutable index = 0
            for f in req.Folderids do
                yield sprintf "folderid[%d]=%O" index f
                index <- index + 1
        }

        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/collections/fave"
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

        let o = FaveResponse.Parse json
        return o.Favourites
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask