namespace DeviantArtFs.Stash

open DeviantArtFs
open FSharp.Data
open System.IO

type internal ContentsResponse = JsonProvider<"""[
{
    "has_more": true,
    "next_offset": 2,
    "results": []
},
{
    "has_more": false,
    "next_offset": null,
    "results": []
}
]""", SampleIsList=true>

module Contents =
    let AsyncExecute token (stackid: int64) = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/stash/%d/contents" stackid
            |> dafs.createRequest token

        let! json = dafs.asyncRead req
        let resp = ContentsResponse.Parse json
        return {
            HasMore = resp.HasMore
            NextOffset = resp.NextOffset
            Results = resp.Results |> Seq.map (fun j -> j.JsonValue.ToString()) |> Seq.map StashMetadata.Parse
        }
    }

    let AsyncGetRoot token = AsyncExecute token 0L

    let ExecuteAsync token stackid = AsyncExecute token stackid |> Async.StartAsTask
    let GetRootAsync token = AsyncGetRoot token |> Async.StartAsTask