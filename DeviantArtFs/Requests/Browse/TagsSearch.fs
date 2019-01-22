namespace DeviantArtFs.Requests.Browse

open DeviantArtFs

type TagsElement = {
    tag_name: string
}

module TagsSearch =
    let AsyncExecute token (tag_name: string) = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/browse/tags/search?tag_name=%s" (dafs.urlEncode tag_name)
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return dafs.parseListOnly (fun _ -> { tag_name = "" }) json |> Seq.map (fun t -> t.tag_name)
    }

    let ExecuteAsync token tag_name = AsyncExecute token tag_name |> Async.StartAsTask