﻿namespace DeviantArtFs.Requests.Browse

open DeviantArtFs
open DeviantArtFs.Interop
open FSharp.Data

type internal TagsElement = JsonProvider<"""{
    "tag_name": "animal"
}""">

module TagsSearch =
    let AsyncExecute token (tag_name: string) = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/browse/tags/search?tag_name=%s" (dafs.urlEncode tag_name)
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return dafs.parseListOnly TagsElement.Parse json
    }

    let ExecuteAsync token tag_name = AsyncExecute token tag_name |> iop.thenMap (fun t -> t.TagName) |> Async.StartAsTask