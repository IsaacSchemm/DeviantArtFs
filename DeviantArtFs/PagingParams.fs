namespace DeviantArtFs

open System

/// An object with parameters for DeviantArt API requests that use paging.
type DeviantArtPagingParams = {
    /// The offset from the start of the results list to start at.
    Offset: int
    /// The number of items to return in this request.
    Limit: Nullable<int>
}