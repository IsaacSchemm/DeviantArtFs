namespace DeviantArtFs.Requests.Stash

open DeviantArtFs

type PublishCategoryTreeRequest(catpath: string) = 
    member __.Catpath = catpath
    member val Filetype = null with get, set
    member val Frequent = false with get, set

module PublishCategoryTree =
    let AsyncExecute token (req: PublishCategoryTreeRequest) = async {
        let query = seq {
            yield sprintf "catpath=%s" (dafs.urlEncode req.Catpath)
            if not (isNull req.Filetype) then
                yield sprintf "filetype=%s" (dafs.urlEncode req.Filetype)
            yield sprintf "frequent=%b" req.Frequent
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/stash/publish/categorytree?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return DeviantArtCategoryList.ParseSeq json
    }

    let ExecuteAsync token req = AsyncExecute token req |> AsyncThen.mapSeq (fun c -> c :> IBclDeviantArtCategory) |> Async.StartAsTask