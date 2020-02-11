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

/// An object that specifies both a DeviantArt access token and additional
/// parameters that will be passed to the API call.
type IDeviantArtAccessTokenWithCommonParameters =
    inherit IDeviantArtAccessToken
    /// Which expanded fields (if any) to include (default none).
    abstract member Expand: DeviantArtObjectExpansion
    /// Whether to include mature content (default false).
    abstract member MatureContent: bool