namespace DeviantArtFs.Api.Browse

open DeviantArtFs

type TagsElement = {
    tag_name: string
}

module TagsSearch =
    let AsyncExecute token (tag_name: string) =
        seq {
            yield sprintf "tag_name=%s" (Dafs.urlEncode tag_name)
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/tags/search"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtListOnlyResponse<TagsElement>>

    let ExecuteAsync token tag_name =
        AsyncExecute token tag_name
        |> Async.StartAsTask