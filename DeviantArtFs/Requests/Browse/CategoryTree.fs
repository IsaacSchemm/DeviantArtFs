namespace DeviantArtFs.Requests.Browse

open DeviantArtFs

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
        return DeviantArtCategoryList.ParseSeq json
    }

    let ExecuteAsync token catpath = AsyncExecute token catpath |> iop.thenMap (fun c -> c :> IBclDeviantArtCategory) |> Async.StartAsTask