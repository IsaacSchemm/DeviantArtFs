namespace DeviantArtFs.Requests.Stash

open DeviantArtFs
open FSharp.Data

module Stack =
    let AsyncExecute token (stackid: int64) = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/stash/%d" stackid
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return StashMetadata.Parse json
    }

    let ExecuteAsync token stackid = AsyncExecute token stackid |> Async.StartAsTask