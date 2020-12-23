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
    new(x: DeviantArtFeedInclude) as this =
        FeedSettingsUpdateRequest()
        then
            this.Statuses <- Nullable x.statuses
            this.Deviations <- Nullable x.deviations
            this.Journals <- Nullable x.journals
            this.GroupDeviations <- Nullable x.group_deviations
            this.Collections <- Nullable x.collections
            this.Misc <- Nullable x.misc

module FeedSettingsUpdate =
    open FSharp.Control

    let AsyncExecute token (req: FeedSettingsUpdateRequest) = async {
        let query = seq {
            if req.Statuses.HasValue then yield sprintf "include[statuses]=%b" req.Statuses.Value
            if req.Deviations.HasValue then yield sprintf "include[deviations]=%b" req.Deviations.Value
            if req.Journals.HasValue then yield sprintf "include[journals]=%b" req.Journals.Value
            if req.GroupDeviations.HasValue then yield sprintf "include[group_deviations]=%b" req.GroupDeviations.Value
            if req.Collections.HasValue then yield sprintf "include[collections]=%b" req.Collections.Value
            if req.Misc.HasValue then yield sprintf "include[misc]=%b" req.Misc.Value
        }

        let req = Dafs.createRequest token DeviantArtCommonParams.Default "https://www.deviantart.com/api/v1/oauth2/feed/settings/update"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"
        req.RequestBodyText <- String.concat "&" query

        let! json = Dafs.asyncRead req
        let resp = DeviantArtSuccessOrErrorResponse.Parse json
        ignore resp
    }

    let ExecuteAsync token req =
        AsyncExecute token req
        |> Async.StartAsTask
        :> System.Threading.Tasks.Task