namespace DeviantArtFs

open System
open System.Net
open System.Collections.Generic

type PagingParams() =
    member val Offset = 0 with get, set
    member val Limit = Nullable<int>() with get, set
    member this.GetQuery() = seq {
        yield sprintf "offset=%d" this.Offset
        if this.Limit.HasValue then
            yield sprintf "limit=%d" this.Limit.Value
    }

type IExtParams =
    abstract member ExtSubmission: bool
    abstract member ExtCamera: bool
    abstract member ExtStats: bool

type ExtParams = {
    ExtSubmission: bool
    ExtCamera: bool
    ExtStats: bool
} with
    static member None = { ExtSubmission = false; ExtCamera = false; ExtStats = false } :> IExtParams
    static member All = { ExtSubmission = true; ExtCamera = true; ExtStats = true } :> IExtParams
    interface IExtParams with
        member this.ExtSubmission = this.ExtSubmission
        member this.ExtCamera = this.ExtCamera
        member this.ExtStats = this.ExtStats

[<RequireQualifiedAccess>]
type FieldChange<'a> =
    | UpdateToValue of 'a
    | NoChange