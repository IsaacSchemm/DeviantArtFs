namespace DeviantArtFs

open System
open System.Net
open FSharp.Data

type IDeviantArtAccessToken =
    abstract member AccessToken: string with get

type IDeviantArtRefreshToken =
    inherit IDeviantArtAccessToken
    abstract member ExpiresAt: DateTimeOffset with get
    abstract member RefreshToken: string with get

type DeviantArtBaseResponse = JsonProvider<"""{"status":"error"}""">

type DeviantArtErrorResponse = JsonProvider<"""{"error":"invalid_request","error_description":"Must provide an access_token to access this resource.","status":"error"}""">

type DeviantArtException(resp: WebResponse, body: DeviantArtErrorResponse.Root) =
    inherit Exception(body.ErrorDescription)

    member __.ResponseBody = body
    member __.StatusCode =
        match resp with
        | :? HttpWebResponse as h -> Nullable h.StatusCode
        | _ -> Nullable()

type ExtParams =
    struct
        val ExtSubmission: bool
        val ExtCamera: bool
        val ExtStats: bool
    end

type UserResult = {
    Userid: Guid
    Username: string
    Usericon: string
    Type: string
}

type DeviantArtPagedResult<'a> = {
    HasMore: bool
    NextOffset: int option
    Results: seq<'a>
}

type DeviantArtBidirectionalPagedResult<'a> = {
    HasMore: bool
    NextOffset: int option
    HasLess: bool
    PrevOffset: int option
    Results: seq<'a>
}

type IDeltaEntry =
    abstract member Itemid: Nullable<int64>
    abstract member Stackid: Nullable<int64>
    abstract member Metadata: string
    abstract member Position: Nullable<int>