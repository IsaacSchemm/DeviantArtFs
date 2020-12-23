namespace DeviantArtFs.Requests.Browse

open DeviantArtFs

type TagsElement = {
    tag_name: string
}

module TagsSearch =
    let AsyncExecute token (tag_name: string) = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/browse/tags/search?tag_name=%s" (Dafs.urlEncode tag_name)
            |> Dafs.createRequest token DeviantArtCommonParams.Default
        let! json = Dafs.asyncRead req
        return DeviantArtListOnlyResponse.ParseList json |> List.map (fun t -> t.tag_name)
    }

    let ExecuteAsync token tag_name =
        AsyncExecute token tag_name
        |> AsyncThen.map List.toSeq
        |> Async.StartAsTask