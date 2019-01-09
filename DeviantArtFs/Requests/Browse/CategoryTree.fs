namespace DeviantArtFs.Requests.Browse

open DeviantArtFs
open DeviantArtFs.Interop
open FSharp.Data

type CategoryTreeResponse = JsonProvider<"""{
    "categories": [
        {
            "catpath": "anthro",
            "title": "Anthro",
            "has_subcategory": true,
            "parent_catpath": "/"
        }
    ]
}""">

module CategoryTree =
    let AsyncExecute token (catpath: string) = async {
        let query = seq {
            if not (isNull catpath) then
                yield sprintf "catpath=%s" (dafs.urlEncode catpath)
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/categorytree?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let o = CategoryTreeResponse.Parse json
        return o.Categories :> seq<CategoryTreeResponse.Category>
    }

    let ExecuteAsync token catpath = Async.StartAsTask (async {
        let! resp = AsyncExecute token catpath
        return resp
            |> Seq.map (fun c -> {
                new ICategory with
                    member __.Catpath = c.Catpath
                    member __.Title = c.Title
                    member __.HasSubcategory = c.HasSubcategory
                    member __.ParentCatpath = c.ParentCatpath
            })
    })