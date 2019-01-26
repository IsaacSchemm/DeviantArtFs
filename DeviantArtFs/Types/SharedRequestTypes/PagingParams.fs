namespace DeviantArtFs

open System

type IPagingParams =
    abstract member Offset: int
    abstract member Limit: Nullable<int>

type PagingParams() =
    member val Offset = 0 with get, set
    member val Limit = Nullable<int>() with get, set
    interface IPagingParams with
        member this.Offset = this.Offset
        member this.Limit = this.Limit