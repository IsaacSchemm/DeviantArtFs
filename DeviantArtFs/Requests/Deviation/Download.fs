namespace DeviantArtFs.Requests.Deviation

open DeviantArtFs
open FSharp.Data
open System

type internal DownloadResponse = JsonProvider<"""{
    "src": "https://www.example.com",
    "width": 640,
    "height": 480,
    "filesize": 10000
}""">

module Download =
    let AsyncExecute token (deviationid: Guid) = async {
        let req = sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/download/%O" deviationid |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let resp = DownloadResponse.Parse json
        return {
            new IBclDeviationDownload with
                member __.Src = resp.Src
                member __.Width = resp.Width
                member __.Height = resp.Height
                member __.Filesize = resp.Filesize
        }
    }

    let ExecuteAsync token deviationid = AsyncExecute token deviationid |> Async.StartAsTask