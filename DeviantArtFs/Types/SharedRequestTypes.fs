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

type ExtParams() =
    member val ExtSubmission = false
    member val ExtCamera = false
    member val ExtStats = false

[<RequireQualifiedAccess>]
type FieldChange<'a> =
    | UpdateToValue of 'a
    | NoChange