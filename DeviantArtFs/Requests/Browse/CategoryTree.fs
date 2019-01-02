namespace DeviantArtFs.Requests.Browse

open DeviantArtFs
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

type CategoryTreeResult = {
    Catpath: string
    Title: string
    HasSubcategory: bool
    ParentCatpath: string
}

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
        let resp = CategoryTreeResponse.Parse json
        return resp.Categories
            |> Seq.map (fun c -> {
                Catpath = c.Catpath
                Title = c.Title
                HasSubcategory = c.HasSubcategory
                ParentCatpath = c.ParentCatpath
            })
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask