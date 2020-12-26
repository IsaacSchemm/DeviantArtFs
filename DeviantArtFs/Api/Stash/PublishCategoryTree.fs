namespace DeviantArtFs.Api.Stash

open DeviantArtFs

type PublishCategoryTreeRequest(catpath: string) = 
    member __.Catpath = catpath
    member val Filetype = null with get, set
    member val Frequent = false with get, set

module PublishCategoryTree =
    let AsyncExecute token (req: PublishCategoryTreeRequest) =
        seq {
            yield sprintf "catpath=%s" (Dafs.urlEncode req.Catpath)
            if not (isNull req.Filetype) then
                yield sprintf "filetype=%s" (Dafs.urlEncode req.Filetype)
            yield sprintf "frequent=%b" req.Frequent
        }
        |> Dafs.createRequest2 token "https://www.deviantart.com/api/v1/oauth2/stash/publish/categorytree"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtCategoryList>

    let ExecuteAsync token req =
        AsyncExecute token req
        |> Async.StartAsTask