namespace DeviantArtFs.Requests.Stash

open DeviantArtFs

type PublishCategoryTreeRequest(catpath: string) = 
    member __.Catpath = catpath
    member val Filetype = null with get, set
    member val Frequent = false with get, set

module PublishCategoryTree =
    let AsyncExecute token (req: PublishCategoryTreeRequest) = async {
        let query = seq {
            yield sprintf "catpath=%s" (Dafs.urlEncode req.Catpath)
            if not (isNull req.Filetype) then
                yield sprintf "filetype=%s" (Dafs.urlEncode req.Filetype)
            yield sprintf "frequent=%b" req.Frequent
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/stash/publish/categorytree?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtCategoryList.ParseList json
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask