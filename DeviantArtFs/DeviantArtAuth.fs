namespace DeviantArtFs

open FSharp.Data
open System.Net
open System.IO
open System.Collections.Generic
open System
open FSharp.Json

type internal TokenResponse = {
  expires_in: int
  status: string
  access_token: string
  token_type: string
  refresh_token: string
  scope: string
} with
    interface IDeviantArtRefreshToken with
        member this.AccessToken = this.access_token
        member this.ExpiresAt = DateTimeOffset.UtcNow.AddSeconds (float this.expires_in)
        member this.RefreshToken = this.refresh_token

type DeviantArtAuth(client_id: int, client_secret: string) =
    let UserAgent = "DeviantArtFs/0.1 (https://github.com/libertyernie/CrosspostSharp"

    let BuildForm (dict: IDictionary<string, string>) =
        let parameters = seq {
            for p in dict do
                if isNull p.Value then
                    failwithf "Null values in form not allowed"
                let key = dafs.urlEncode p.Key
                let value = dafs.urlEncode (p.Value.ToString())
                yield sprintf "%s=%s" key value
        }
        String.concat "&" parameters

    member __.AsyncGetToken (code: string) (redirect_uri: Uri) = async {
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
                    ("client_id", client_id.ToString());
                    ("client_secret", client_secret);
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
        let obj = Json.deserialize<TokenResponse> json
        if obj.status <> "success" then
            failwithf "An unknown error occured"
        if obj.token_type <> "Bearer" then
            failwithf "token_type was not Bearer"
        return obj :> IDeviantArtRefreshToken
    }

    member __.AsyncRefresh (refresh_token: string) = async {
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
                    ("client_id", client_id.ToString());
                    ("client_secret", client_secret);
                    ("grant_type", "refresh_token");
                    ("refresh_token", refresh_token)
                ]
                |> dict
                |> BuildForm
                |> sw.WriteAsync
                |> Async.AwaitTask
        }

        use! resp = req.GetResponseAsync() |> Async.AwaitTask
        use sr = new StreamReader(resp.GetResponseStream())
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask
        let obj = Json.deserialize<TokenResponse> json
        if obj.status <> "success" then
            failwithf "An unknown error occured"
        if obj.token_type <> "Bearer" then
            failwithf "token_type was not Bearer"
        return obj :> IDeviantArtRefreshToken
    }

    member this.GetTokenAsync code redirect_uri =
        this.AsyncGetToken code redirect_uri |> Async.StartAsTask
    member this.RefreshAsync refresh_token =
        this.AsyncRefresh refresh_token |> Async.StartAsTask