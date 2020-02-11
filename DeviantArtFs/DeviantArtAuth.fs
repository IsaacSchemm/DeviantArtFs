namespace DeviantArtFs

open System.Net
open System.IO
open System.Collections.Generic
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
    interface IDeviantArtRefreshToken with
        member this.AccessToken = this.access_token
        member this.RefreshToken = this.refresh_token

/// A module that provides methods to obtain tokens from the DeviantArt API.
module DeviantArtAuth =
    let UserAgent = "DeviantArtFs/5.0 (https://github.com/IsaacSchemm/DeviantArtFs)"

    /// Checks whether the given WebResponse has the given HTTP status code.
    let IsStatus (code: int) (response: WebResponse) =
        match response with
        | :? HttpWebResponse as h -> int h.StatusCode = code
        | _ -> false

    /// Makes a URL-encoded query string for the given map of keys and values.
    let BuildForm (dict: IDictionary<string, string>) =
        let parameters = seq {
            for p in dict do
                if isNull p.Value then
                    failwithf "Null values in form not allowed"
                let key = Uri.EscapeDataString p.Key
                let value = Uri.EscapeDataString (p.Value.ToString())
                yield sprintf "%s=%s" key value
        }
        String.concat "&" parameters

    /// Get a new token from the server, using an authorization code.
    let AsyncGetToken (app: DeviantArtApp) (code: string) (redirect_uri: Uri) = async {
        if isNull code then
            nullArg "code"
        if isNull redirect_uri then
            nullArg "redirect_uri"

        let req = WebRequest.CreateHttp "https://www.deviantart.com/oauth2/token"
        req.UserAgent <- UserAgent
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"
            
        do! async {
            use! reqStream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(reqStream)
            do!
                [
                    ("client_id", app.client_id);
                    ("client_secret", app.client_secret);
                    ("grant_type", "authorization_code");
                    ("code", code);
                    ("redirect_uri", redirect_uri.AbsoluteUri)
                ]
                |> dict
                |> BuildForm
                |> sw.WriteAsync
                |> Async.AwaitTask
        }

        use! resp = req.GetResponseAsync() |> Async.AwaitTask
        use sr = new StreamReader(resp.GetResponseStream())
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask
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

        let req = WebRequest.CreateHttp "https://www.deviantart.com/oauth2/token"
        req.UserAgent <- UserAgent
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"
            
        do! async {
            use! reqStream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(reqStream)
            do!
                [
                    ("client_id", app.client_id);
                    ("client_secret", app.client_secret);
                    ("grant_type", "refresh_token");
                    ("refresh_token", refresh_token)
                ]
                |> dict
                |> BuildForm
                |> sw.WriteAsync
                |> Async.AwaitTask
        }

        try
            use! resp = req.AsyncGetResponse()
            use sr = new StreamReader(resp.GetResponseStream())
            let! json = sr.ReadToEndAsync() |> Async.AwaitTask
            let obj = Json.deserialize<DeviantArtTokenResponse> json
            if obj.status <> "success" then
                failwithf "An unknown error occured"
            if obj.token_type <> "Bearer" then
                failwithf "token_type was not Bearer"
            return obj
        with
        | :? WebException as ex when IsStatus 400 ex.Response ->
            return raise (new InvalidRefreshTokenException(ex))
    }

    /// Revoke a refresh token or access token.
    let AsyncRevoke (token: string) (revoke_refresh_only: bool) = async {
        if isNull token then
            nullArg "token"

        let req = WebRequest.CreateHttp "https://www.deviantart.com/oauth2/revoke"
        req.UserAgent <- UserAgent
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        let query = seq {
            yield token |> Uri.EscapeDataString |> sprintf "token=%s"
            if revoke_refresh_only then
                yield "revoke_refresh_only=true"
        }
            
        do! async {
            use! reqStream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(reqStream)
            do! String.concat "&" query |> sw.WriteAsync |> Async.AwaitTask
        }

        use! resp = req.AsyncGetResponse()
        use sr = new StreamReader(resp.GetResponseStream())
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask
        ignore json
    }

    /// Get a new token from the server, using an authorization code.
    let GetTokenAsync app code redirect_uri =
        AsyncGetToken app code redirect_uri |> Async.StartAsTask
    /// Get a new token from the server, using a refresh token.
    let RefreshAsync app refresh_token =
        AsyncRefresh app refresh_token |> Async.StartAsTask
    /// Revoke a refresh token or access token.
    let RevokeAsync token revoke_refresh_only =
        AsyncRevoke token revoke_refresh_only |> Async.StartAsTask :> System.Threading.Tasks.Task