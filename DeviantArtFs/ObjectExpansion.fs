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