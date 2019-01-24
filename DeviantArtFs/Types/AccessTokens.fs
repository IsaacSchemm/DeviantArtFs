namespace DeviantArtFs

open System

type IDeviantArtAccessToken =
    abstract member AccessToken: string with get

type IDeviantArtRefreshToken =
    inherit IDeviantArtAccessToken
    abstract member ExpiresAt: DateTimeOffset with get
    abstract member RefreshToken: string with get

[<Flags>]
type DeviantArtObjectExpansion =
| None = 0
| UserDetails = 1
| UserGeo = 2
| UserProfile = 4
| UserStats = 8
| UserWatch = 16

type IDeviantArtCommonParameters =
    inherit IDeviantArtAccessToken
    abstract member Expand: DeviantArtObjectExpansion
    abstract member MatureContent: bool