namespace DeviantArtFs.Requests.Stash

open DeviantArtFs
open FSharp.Data
open System.IO

module Position =
    let AsyncExecute token (stackid: int64) (position: int) = async {
        let query = seq {
            yield sprintf "position=%d" position
        }

        let req = sprintf "https://www.deviantart.com/api/v1/oauth2/stash/position/%d" stackid |> dafs.createRequest token
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        do! async {
            use! stream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(stream)
            do! String.concat "&" query |> sw.WriteAsync |> Async.AwaitTask
        }

        let! json = dafs.asyncRead req
        SuccessOrErrorResponse.Parse json |> dafs.assertSuccess
    }

    let ExecuteAsync token stackid position = AsyncExecute token stackid position |> Async.StartAsTask