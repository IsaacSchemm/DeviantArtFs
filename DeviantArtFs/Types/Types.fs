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

type ExtParams =
    struct
        val ExtSubmission: bool
        val ExtCamera: bool
        val ExtStats: bool
    end

//type ListOnly<'a> = {
//    Results: seq<'a>
//} with
//    interface IEnumerable<'a> with
//        member this.GetEnumerator() = this.Results.GetEnumerator() :> System.Collections.IEnumerator
//        member this.GetEnumerator() = this.Results.GetEnumerator()

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