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

type IDeviantArtAccessTokenWithCommonParameters =
    inherit IDeviantArtAccessToken
    abstract member Expand: DeviantArtObjectExpansion
    abstract member MatureContent: bool

type DeviantArtCommonParameters() =
    member val Expand = DeviantArtObjectExpansion.None with get, set
    member val MatureContent = false with get, set
    member this.WrapToken (token: IDeviantArtAccessToken) = {
        new IDeviantArtAccessTokenWithCommonParameters with
            member __.AccessToken = token.AccessToken
            member __.Expand = this.Expand
            member __.MatureContent = this.MatureContent
    }