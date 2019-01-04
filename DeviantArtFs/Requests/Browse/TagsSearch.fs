namespace DeviantArtFs.Requests.Browse

open DeviantArtFs
open FSharp.Data

type internal TagsResponse = JsonProvider<"""{
    "results": [
        {
            "tag_name": "animal"
        },
        {
            "tag_name": "animation"
        },
        {
            "tag_name": "anime"
        }
    ]
}""">

module TagsSearch =
    let AsyncExecute token (tag_name: string) = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/browse/tags/search?tag_name=%s" (dafs.urlEncode tag_name)
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return TagsResponse.Parse json
    }

    let ExecuteAsync token tag_name = Async.StartAsTask (async {
        let! o = AsyncExecute token tag_name
        return o.Results |> Seq.map (fun t -> t.TagName)
    })