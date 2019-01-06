namespace DeviantArtFs.Requests.Stash

open DeviantArtFs
open DeviantArtFs.Interop

module Contents =
    let AsyncExecute token (stackid: int64) = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/stash/%d/contents" stackid
            |> dafs.createRequest token

        let! json = dafs.asyncRead req
        return dafs.parsePage StashMetadataResponse.Parse json
    }

    let AsyncGetRoot token = AsyncExecute token 0L

    let ExecuteAsync token stackid =
        AsyncExecute token stackid
        |> iop.thenMapResult StashMetadata
        |> Async.StartAsTask
    let GetRootAsync token =
        AsyncGetRoot token
        |> iop.thenMapResult StashMetadata
        |> Async.StartAsTask