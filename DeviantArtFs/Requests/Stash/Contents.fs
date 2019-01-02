namespace DeviantArtFs.Requests.Stash

open DeviantArtFs
open FSharp.Data
open System.IO

module Contents =
    let AsyncExecute token (stackid: int64) = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/stash/%d/contents" stackid
            |> dafs.createRequest token

        let! json = dafs.asyncRead req
        let resp = GenericListResponse.Parse json
        return {
            HasMore = resp.HasMore
            NextOffset = resp.NextOffset
            Results = resp.Results |> Seq.map (fun j -> j.JsonValue.ToString()) |> Seq.map StashMetadata.Parse
        }
    }

    let AsyncGetRoot token = AsyncExecute token 0L

    let ExecuteAsync token stackid = AsyncExecute token stackid |> Async.StartAsTask
    let GetRootAsync token = AsyncGetRoot token |> Async.StartAsTask