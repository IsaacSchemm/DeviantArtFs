﻿namespace DeviantArtFs.Requests.Stash

open DeviantArtFs
open DeviantArtFs.Interop
open FSharp.Data

type PublishCategoryTreeResponse = JsonProvider<"""{
    "categories": [
        {
            "catpath": "anthro",
            "title": "Anthro",
            "has_subcategory": true,
            "parent_catpath": "/"
        }
    ]
}""">

type PublishCategoryTreeResult = {
    Catpath: string
    Title: string
    HasSubcategory: bool
    ParentCatpath: string
}

type PublishCategoryTreeRequest() = 
    member val Catpath = "/" with get, set
    member val Filetype = null with get, set
    member val Frequent = false with get, set

module PublishCategoryTree =
    let AsyncExecute token (req: PublishCategoryTreeRequest) = async {
        let query = seq {
            yield sprintf "catpath=%s" (dafs.urlEncode req.Catpath)
            yield sprintf "filetype=%s" (dafs.urlEncode req.Filetype)
            yield sprintf "frequent=%b" req.Frequent
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/stash/publish/categorytree?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return PublishCategoryTreeResponse.Parse json
    }

    let ExecuteAsync token req = Async.StartAsTask (async {
        let! resp = AsyncExecute token req
        return resp.Categories
            |> Seq.map (fun c -> {
                new ICategory with
                    member __.Catpath = c.Catpath
                    member __.Title = c.Title
                    member __.HasSubcategory = c.HasSubcategory
                    member __.ParentCatpath = c.ParentCatpath
            })
    })