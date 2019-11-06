namespace DeviantArtFs

open System
open System.Threading.Tasks

/// An object that contains an access token for the DeviantArt API.
type IDeviantArtAccessToken =
    /// An access token for the DeviantArt API.
    abstract member AccessToken: string with get

/// An object that contains an access token and a refresh token for the
/// DeviantArt API.
type IDeviantArtRefreshToken =
    inherit IDeviantArtAccessToken
    /// A refresh token for the DeviantArt API that can be used to get a new
    /// set of tokens when the access token expires.
    abstract member RefreshToken: string with get

/// A set of tokens obtained from the DeviantArt API, including an expiration
/// date and scope information.
type IDeviantArtRefreshTokenFull =
    inherit IDeviantArtRefreshToken
    /// The date and time at which the access token expires.
    abstract member ExpiresAt: DateTimeOffset with get
    /// A list of permissions for this token.
    abstract member Scopes: seq<string> with get

/// An object that connects to DeviantArt to get a new set of API tokens.
type IDeviantArtAuth =
    /// Get a new set of API tokens, given a refresh token.
    abstract member AsyncRefresh: string -> Async<IDeviantArtRefreshTokenFull>

/// An object that holds DeviantArt API tokens and provides a method to update
/// its backing store with new tokens when needed.
type IDeviantArtAutomaticRefreshToken =
    inherit IDeviantArtRefreshToken
    /// An object that can get a new set of API tokens from DeviantArt. This
    /// is typically an object of the class "DeviantArtAuth", created using
    /// a client ID and client secret.
    abstract member DeviantArtAuth: IDeviantArtAuth with get
    /// A function that takes a new set of tokens and saves them, both in this
    /// object and in the backing store (if any).
    abstract member UpdateTokenAsync: IDeviantArtRefreshToken -> Task

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

/// A class that contains additional parameters for DeviantArt API calls and
/// lets you wrap your IDeviantArtAccessToken object with them.
type DeviantArtCommonParameters() =
    /// Which expanded fields (if any) to include (default none).
    member val Expand = DeviantArtObjectExpansion.None with get, set
    /// Whether to include mature content (default false).
    member val MatureContent = false with get, set
    /// Create an IDeviantArtAccessToken object that includes the additional
    /// parameters defined in this object.
    member this.WrapToken (token: IDeviantArtAccessToken) = {
        new IDeviantArtAccessTokenWithCommonParameters with
            member __.AccessToken = token.AccessToken
            member __.Expand = this.Expand
            member __.MatureContent = this.MatureContent
    }