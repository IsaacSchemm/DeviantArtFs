namespace DeviantArtFs.Requests.Browse

open DeviantArtFs

module CategoryTree =
    let AsyncExecute token (catpath: string) = async {
        let query = seq {
            if not (isNull catpath) then
                yield sprintf "catpath=%s" (Dafs.urlEncode catpath)
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/categorytree?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtCategoryList.ParseList json
    }

    let ExecuteAsync token catpath = AsyncExecute token catpath |> Async.StartAsTask