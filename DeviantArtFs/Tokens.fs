namespace DeviantArtFs

open System.Threading.Tasks

/// A DeviantArt app that connects via OAuth.
type DeviantArtApp = {
    client_id: string
    client_secret: string
}

[<RequireQualifiedAccess>]
type Expansion = CommentFullText | DeviationPinned | DeviationFullText | StatusFullText | UserDetails | UserGeo | UserProfile | UserStats | UserWatch

[<RequireQualifiedAccess>]
type ExtParam = Submission | Camera | Stats | Collection | Gallery

[<RequireQualifiedAccess>]
type OptionalParameter = Expansion of seq<Expansion> | ExtParam of ExtParam | MatureContent of bool | CustomParameter of string * string
with static member None = Seq.empty<OptionalParameter>

/// An object that contains an access token for the DeviantArt API.
type IDeviantArtAccessToken =
    /// An access token for the DeviantArt API.
    abstract member AccessToken: string with get

/// An object that contains an access token and additional query parameters for the DeviantArt API.
type IDeviantArtAccessTokenWithOptionalParameters =
    inherit IDeviantArtAccessToken
    /// A set of parameters to include with each request.
    abstract member OptionalParameters: seq<OptionalParameter>

/// An object that contains an access token for the DeviantArt API and can
/// fetch an additional token if the main token is invalid.
type IDeviantArtRefreshableAccessToken =
    inherit IDeviantArtAccessToken
    /// Updates the access token, or does nothing if no new token is
    /// available.
    abstract member RefreshAccessTokenAsync: unit -> Task
