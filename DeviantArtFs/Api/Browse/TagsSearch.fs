namespace DeviantArtFs.Api.Browse

open DeviantArtFs

type TagsElement = {
    tag_name: string
}

module TagsSearch =
    let AsyncExecute token common (tag_name: string) = async {
        let query = seq {
            yield sprintf "tag_name=%s" (Dafs.urlEncode tag_name)
            yield! QueryFor.commonParams common
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/tags/search?tag_name=%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtListOnlyResponse.ParseList json |> List.map (fun t -> t.tag_name)
    }

    let ExecuteAsync token common tag_name =
        AsyncExecute token common tag_name
        |> AsyncThen.map List.toSeq
        |> Async.StartAsTask