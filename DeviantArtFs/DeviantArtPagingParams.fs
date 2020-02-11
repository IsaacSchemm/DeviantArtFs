namespace DeviantArtFs

open System

/// An object with parameters for DeviantArt API requests that use paging.
type IDeviantArtPagingParams =
    /// The offset from the start of the results list to start at.
    abstract member Offset: int
    /// The number of items to return in this request.
    abstract member Limit: Nullable<int>

/// An object with parameters for DeviantArt API requests that use paging.
type DeviantArtPagingParams() =
    /// The offset from the start of the results list to start at.
    member val Offset = 0 with get, set
    /// The number of items to return in this request.
    member val Limit = Nullable<int>() with get, set
    interface IDeviantArtPagingParams with
        member this.Offset = this.Offset
        member this.Limit = this.Limit