namespace DeviantArtFs.Api.Stash

open DeviantArtFs

type PublishCategoryTreeRequest(catpath: string) = 
    member __.Catpath = catpath
    member val Filetype = null with get, set
    member val Frequent = false with get, set

module PublishCategoryTree =
    let AsyncExecute token common (req: PublishCategoryTreeRequest) =
        seq {
            yield sprintf "catpath=%s" (Dafs.urlEncode req.Catpath)
            if not (isNull req.Filetype) then
                yield sprintf "filetype=%s" (Dafs.urlEncode req.Filetype)
            yield sprintf "frequent=%b" req.Frequent
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/stash/publish/categorytree"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtCategoryList>

    let ExecuteAsync token common req =
        AsyncExecute token common req
        |> Async.StartAsTask