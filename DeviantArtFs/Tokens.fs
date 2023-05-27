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

/// An object that contains an access token for the DeviantArt API and can
/// fetch an additional token if the main token is invalid.
type IDeviantArtRefreshableAccessToken =
    inherit IDeviantArtAccessToken
    /// Updates the access token, or does nothing if no new token is
    /// available.
    abstract member RefreshAccessTokenAsync: unit -> Task
