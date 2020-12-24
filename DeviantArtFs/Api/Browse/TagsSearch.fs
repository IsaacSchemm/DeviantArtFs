namespace DeviantArtFs.Api.Browse

open DeviantArtFs

type TagsElement = {
    tag_name: string
}

module TagsSearch =
    let AsyncExecute token common (tag_name: string) =
        seq {
            yield sprintf "tag_name=%s" (Dafs.urlEncode tag_name)
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest2 token "https://www.deviantart.com/api/v1/oauth2/browse/tags/search"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtListOnlyResponse<TagsElement>>
        |> Dafs.extractResults
        |> AsyncThen.mapSeq (fun t -> t.tag_name)

    let ExecuteAsync token common tag_name =
        AsyncExecute token common tag_name
        |> Async.StartAsTask