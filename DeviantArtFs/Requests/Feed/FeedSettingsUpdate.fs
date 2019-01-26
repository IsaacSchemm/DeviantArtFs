namespace DeviantArtFs.Requests.Feed

open DeviantArtFs
open System

type FeedSettingsUpdateRequest() =
    member val Statuses = Nullable<bool>() with get, set
    member val Deviations = Nullable<bool>() with get, set
    member val Journals = Nullable<bool>() with get, set
    member val GroupDeviations = Nullable<bool>() with get, set
    member val Collections = Nullable<bool>() with get, set
    member val Misc = Nullable<bool>() with get, set

    // For convenience, you can create an DeviantArtFeedInclude record and pass it into this constructor to update all settings
    new(x: IBclDeviantArtFeedInclude) as this =
        FeedSettingsUpdateRequest()
        then
            this.Statuses <- Nullable x.Statuses
            this.Deviations <- Nullable x.Deviations
            this.Journals <- Nullable x.Journals
            this.GroupDeviations <- Nullable x.GroupDeviations
            this.Collections <- Nullable x.Collections
            this.Misc <- Nullable x.Misc

module FeedSettingsUpdate =
    open FSharp.Control
    open System.IO

    let AsyncExecute token (req: FeedSettingsUpdateRequest) = async {
        let query = seq {
            if req.Statuses.HasValue then yield sprintf "include[statuses]=%b" req.Statuses.Value
            if req.Deviations.HasValue then yield sprintf "include[deviations]=%b" req.Deviations.Value
            if req.Journals.HasValue then yield sprintf "include[journals]=%b" req.Journals.Value
            if req.GroupDeviations.HasValue then yield sprintf "include[group_deviations]=%b" req.GroupDeviations.Value
            if req.Collections.HasValue then yield sprintf "include[collections]=%b" req.Collections.Value
            if req.Misc.HasValue then yield sprintf "include[misc]=%b" req.Misc.Value
        }

        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/feed/settings/update"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        do! async {
            use! stream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(stream)
            do! String.concat "&" query |> sw.WriteAsync |> Async.AwaitTask
        }

        let! json = dafs.asyncRead req
        let resp = DeviantArtSuccessOrErrorResponse.Parse json
        dafs.assertSuccess resp
    }

    let ExecuteAsync token req =
        AsyncExecute token req
        |> Async.StartAsTask
        :> System.Threading.Tasks.Task