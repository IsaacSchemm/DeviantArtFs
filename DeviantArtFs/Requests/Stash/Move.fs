namespace DeviantArtFs.Requests.Stash

open DeviantArtFs
open DeviantArtFs.Interop
open FSharp.Data
open System.IO

type internal MoveResponse = JsonProvider<"""{
    "target": {},
    "changes": []
}""">

type MoveResult = {
    Target: StashMetadata.Root
    Changes: seq<StashMetadata.Root>
} with
    interface IMoveResult with
        member this.Target = this.Target.JsonValue.ToString()
        member this.Changes = this.Changes |> Seq.map (fun m -> m.JsonValue.ToString())

module Move =
    let AsyncExecute token (stackid: int64) (targetid: int64) = async {
        let query = seq {
            yield sprintf "targetid=%d" targetid
        }

        let req = sprintf "https://www.deviantart.com/api/v1/oauth2/stash/move/%d" stackid |> dafs.createRequest token
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        do! async {
            use! stream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(stream)
            do! String.concat "&" query |> sw.WriteAsync |> Async.AwaitTask
        }

        let! json = dafs.asyncRead req
        let resp = MoveResponse.Parse json
        return {
            Target = resp.Target.JsonValue.ToString() |> StashMetadata.Parse
            Changes = resp.Changes |> Seq.map (fun j -> j.JsonValue.ToString()) |> Seq.map StashMetadata.Parse
        }
    }

    let ExecuteAsync token stackid targetid = AsyncExecute token stackid targetid |> iop.thenTo (fun r -> r :> IMoveResult) |> Async.StartAsTask