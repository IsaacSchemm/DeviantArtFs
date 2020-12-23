namespace DeviantArtFs

open System

/// Common object expansion parameters for DeviantArt user requests.
[<Flags>]
type DeviantArtObjectExpansion =
| None = 0
| UserDetails = 1
| UserGeo = 2
| UserProfile = 4
| UserStats = 8
| UserWatch = 16
| All = -1

/// An object with common parameters for DeviantArt API requests.
type DeviantArtCommonParams = {
    /// Which expanded fields (if any) to include (default none).
    Expand: DeviantArtObjectExpansion
    /// Whether to include mature content (default false).
    MatureContent: bool
} with
    static member Default = {
        Expand = DeviantArtObjectExpansion.None
        MatureContent = false
    }