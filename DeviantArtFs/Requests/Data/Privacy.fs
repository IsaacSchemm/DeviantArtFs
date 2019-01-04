namespace DeviantArtFs.Requests.Data

open DeviantArtFs
open DeviantArtFs.Interop
open FSharp.Data

module Privacy =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/data/privacy"
        let! json = dafs.asyncRead req
        return TextOnlyResponse.Parse json
    }

    let ExecuteAsync token = AsyncExecute token |> iop.thenTo (fun obj -> obj.Text) |> Async.StartAsTask