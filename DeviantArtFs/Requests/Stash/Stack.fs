namespace DeviantArtFs.Requests.Stash

open DeviantArtFs
open DeviantArtFs.Interop
open FSharp.Data

module Stack =
    let AsyncExecute token (stackid: int64) = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/stash/%d" stackid
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return StashMetadata.Parse json
    }

    let ExecuteAsync token stackid = AsyncExecute token stackid |> iop.thenTo (fun m -> m.JsonValue.ToString()) |> Async.StartAsTask