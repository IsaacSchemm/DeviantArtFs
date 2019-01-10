namespace DeviantArtFs

open System
open System.Net
open System.Collections.Generic

type IDeviantArtAccessToken =
    abstract member AccessToken: string with get

type IDeviantArtRefreshToken =
    inherit IDeviantArtAccessToken
    abstract member ExpiresAt: DateTimeOffset with get
    abstract member RefreshToken: string with get

type DeviantArtException(resp: WebResponse, body: DeviantArtErrorResponse.Root) =
    inherit Exception(body.ErrorDescription)

    member __.ResponseBody = body
    member __.StatusCode =
        match resp with
        | :? HttpWebResponse as h -> Nullable h.StatusCode
        | _ -> Nullable()

type DeviantArtRateLimitException() =
    inherit Exception()

type PagingParams() =
    member val Offset = 0 with get, set
    member val Limit = Nullable<int>() with get, set
    member this.GetQuery() = seq {
        yield sprintf "offset=%d" this.Offset
        if this.Limit.HasValue then
            yield sprintf "limit=%d" this.Limit.Value
    }

type ExtParams() =
    member val ExtSubmission = false
    member val ExtCamera = false
    member val ExtStats = false

[<RequireQualifiedAccess>]
type FieldChange<'a> =
    | UpdateToValue of 'a
    | NoChange

type WatchInfo = {
    Friend: bool
    Deviations: bool
    Journals: bool
    ForumThreads: bool
    Critiques: bool
    Scraps: bool
    Activity: bool
    Collections: bool
}

type IDeltaEntry =
    abstract member Itemid: Nullable<int64>
    abstract member Stackid: Nullable<int64>
    abstract member MetadataJson: string
    abstract member Position: Nullable<int>