namespace DeviantArtFs

open System.Threading.Tasks

/// A DeviantArt app that connects via OAuth.
type DeviantArtApp = {
    client_id: string
    client_secret: string
}

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

/// An object that holds DeviantArt API tokens and provides a method to update
/// its backing store with new tokens when needed.
type IDeviantArtAutomaticRefreshToken =
    inherit IDeviantArtRefreshToken
    /// An object containing a client ID and client secret.
    abstract member App: DeviantArtApp with get
    /// A function that takes a new set of tokens and saves them, both in this
    /// object and in the backing store (if any).
    abstract member UpdateTokenAsync: IDeviantArtRefreshToken -> Task