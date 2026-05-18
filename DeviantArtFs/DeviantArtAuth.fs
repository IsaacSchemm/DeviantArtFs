namespace DeviantArtFs

open System.Net
open System.Net.Http
open System
open FSharp.Json

/// A response from the DeviantArt API that contains new tokens.
type DeviantArtTokenResponse = {
    expires_in: int
    status: string
    access_token: string
    token_type: string
    refresh_token: string
    scope: string
} with
    interface IDeviantArtAccessToken with
        member this.AccessToken = this.access_token

/// A module that provides methods to obtain tokens from the DeviantArt API.
module DeviantArtAuth =
    /// Get a new token from the server, using an authorization code.
    let AsyncGetToken (app: DeviantArtApp) (code: string) (redirect_uri: Uri) = async {
        if isNull code then
            nullArg "code"
        if isNull redirect_uri then
            nullArg "redirect_uri"

        use reqMessage = new HttpRequestMessage(HttpMethod.Post, "https://www.deviantart.com/oauth2/token")
        reqMessage.Content <- DeviantArtHttp.createForm [
            ("client_id", app.client_id)
            ("client_secret", app.client_secret)
            ("grant_type", "authorization_code")
            ("code", code)
            ("redirect_uri", redirect_uri.AbsoluteUri)
        ]

        use! respMessage = DeviantArtHttp.HttpClient.SendAsync(reqMessage) |> Async.AwaitTask

        let! json =
            respMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync()
            |> Async.AwaitTask

        let obj = Json.deserialize<DeviantArtTokenResponse> json

        if obj.status <> "success" then
            failwithf "An unknown error occured"
        if obj.token_type <> "Bearer" then
            failwithf "token_type was not Bearer"

        return obj
    }

    /// Get a new token from the server, using a refresh token.
    let AsyncRefresh (app: DeviantArtApp) (refresh_token: string) = async {
        if isNull refresh_token then
            nullArg "refresh_token"

        use reqMessage = new HttpRequestMessage(HttpMethod.Post, "https://www.deviantart.com/oauth2/token")
        reqMessage.Content <- DeviantArtHttp.createForm [
            ("client_id", app.client_id)
            ("client_secret", app.client_secret)
            ("grant_type", "refresh_token")
            ("refresh_token", refresh_token)
        ]

        use! respMessage = DeviantArtHttp.HttpClient.SendAsync(reqMessage) |> Async.AwaitTask
        if respMessage.StatusCode = HttpStatusCode.BadRequest then
            raise (new InvalidRefreshTokenException(respMessage))

        let! json =
            respMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync()
            |> Async.AwaitTask

        let obj = Json.deserialize<DeviantArtTokenResponse> json

        if obj.status <> "success" then
            failwithf "An unknown error occured"
        if obj.token_type <> "Bearer" then
            failwithf "token_type was not Bearer"

        return obj
    }

    /// Revoke a refresh token or access token.
    let AsyncRevoke (token: string) (revoke_refresh_only: bool) = async {
        if isNull token then
            nullArg "token"

        use reqMessage = new HttpRequestMessage(HttpMethod.Post, "https://www.deviantart.com/oauth2/revoke")
        reqMessage.Content <- DeviantArtHttp.createForm [
            ("token",token)
            if revoke_refresh_only then
                ("revoke_refresh_only", "true")
        ]

        use! respMessage =
            DeviantArtHttp.HttpClient.SendAsync(reqMessage)
            |> Async.AwaitTask

        if respMessage.StatusCode = HttpStatusCode.BadRequest then
            raise (new InvalidRefreshTokenException(respMessage))

        ignore (respMessage.EnsureSuccessStatusCode())
    }

    /// Get a new token from the server, using an authorization code.
    let GetTokenAsync app code redirect_uri cancellationToken =
        Async.StartAsTask (AsyncGetToken app code redirect_uri, cancellationToken = cancellationToken)

    /// Get a new token from the server, using a refresh token.
    let RefreshAsync app refresh_token cancellationToken =
        Async.StartAsTask (AsyncRefresh app refresh_token, cancellationToken = cancellationToken)

    /// Revoke a refresh token or access token.
    let RevokeAsync token revoke_refresh_only cancellationToken =
        Async.StartAsTask (AsyncRevoke token revoke_refresh_only, cancellationToken = cancellationToken)
