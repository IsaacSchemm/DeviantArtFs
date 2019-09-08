namespace DeviantArtFs

open System.Net
open System.IO
open System.Collections.Generic
open System
open FSharp.Json

type DeviantArtTokenResponse = {
  expires_in: int
  status: string
  access_token: string
  token_type: string
  refresh_token: string
  scope: string
} with
    interface IDeviantArtRefreshTokenFull with
        member this.AccessToken = this.access_token
        member this.ExpiresAt = DateTimeOffset.UtcNow.AddSeconds (float this.expires_in)
        member this.RefreshToken = this.refresh_token
        member this.Scopes = this.scope.Split(' ') :> seq<string>

type DeviantArtAuth(client_id: int, client_secret: string) =
    let UserAgent = DeviantArtRequest.UserAgent

    let isStatus (code: int) (response: WebResponse) =
        match response with
        | :? HttpWebResponse as h -> int h.StatusCode = code
        | _ -> false

    let BuildForm (dict: IDictionary<string, string>) =
        let parameters = seq {
            for p in dict do
                if isNull p.Value then
                    failwithf "Null values in form not allowed"
                let key = Dafs.urlEncode p.Key
                let value = Dafs.urlEncode (p.Value.ToString())
                yield sprintf "%s=%s" key value
        }
        String.concat "&" parameters

    member __.ClientId = client_id
    member __.ClientSecret = client_secret

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
        let obj = Json.deserialize<DeviantArtTokenResponse> json
        if obj.status <> "success" then
            failwithf "An unknown error occured"
        if obj.token_type <> "Bearer" then
            failwithf "token_type was not Bearer"
        return obj :> IDeviantArtRefreshTokenFull
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

        try
            use! resp = req.AsyncGetResponse()
            use sr = new StreamReader(resp.GetResponseStream())
            let! json = sr.ReadToEndAsync() |> Async.AwaitTask
            let obj = Json.deserialize<DeviantArtTokenResponse> json
            if obj.status <> "success" then
                failwithf "An unknown error occured"
            if obj.token_type <> "Bearer" then
                failwithf "token_type was not Bearer"
            return obj :> IDeviantArtRefreshTokenFull
        with
        | :? WebException as ex when isStatus 400 ex.Response ->
            return raise (new InvalidRefreshTokenException(ex))
    }

    static member AsyncRevoke (token: string) (revoke_refresh_only: bool) = async {
        if isNull token then
            nullArg "token"

        let req = WebRequest.CreateHttp "https://www.deviantart.com/oauth2/revoke"
        req.UserAgent <- DeviantArtRequest.UserAgent
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        let query = seq {
            yield token |> Dafs.urlEncode |> sprintf "token=%s"
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

    member this.GetTokenAsync code redirect_uri =
        this.AsyncGetToken code redirect_uri |> Async.StartAsTask
    member this.RefreshAsync refresh_token =
        this.AsyncRefresh refresh_token |> Async.StartAsTask
    static member RevokeAsync token revoke_refresh_only =
        DeviantArtAuth.AsyncRevoke token revoke_refresh_only |> Async.StartAsTask :> System.Threading.Tasks.Task

    interface IDeviantArtAuth with
        member this.AsyncRefresh refresh_token = this.AsyncRefresh refresh_token