namespace DeviantArtFs.Requests.Browse

open DeviantArtFs
open DeviantArtFs.Interop
open FSharp.Data

type internal CategoryTreeResponse = JsonProvider<"""{
    "categories": [
        {
            "catpath": "anthro",
            "title": "Anthro",
            "has_subcategory": true,
            "parent_catpath": "/"
        }
    ]
}""">

type CategoryTreeRequest() = 
    member val Catpath = "/" with get, set

module CategoryTree =
    let AsyncExecute token (req: CategoryTreeRequest) = async {
        let query = seq {
            yield sprintf "catpath=%s" (dafs.urlEncode req.Catpath)
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/categorytree?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return CategoryTreeResponse.Parse json
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