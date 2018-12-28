namespace DeviantArtFs

open System.Net
open System.IO
open FSharp.Data

type internal StashUpdateResponse = JsonProvider<"""[{ "success": false, "error_description": "str" }, { "success": true }]""", SampleIsList=true>

module internal dafs =
    let whenDone (f: 'a -> 'b) (workflow: Async<'a>) = async {
        let! result = workflow
        return f result
    }

    let assertStashSuccess (resp: StashUpdateResponse.Root) =
        match (resp.Success, resp.ErrorDescription) with
        | (true, None) -> ()
        | _ -> failwithf "%s" (resp.ErrorDescription |> Option.defaultValue "An unknown error occurred.")

    let urlEncode = WebUtility.UrlEncode

    let userAgent = "DeviantArtFs/0.1 (https://github.com/libertyernie/CrosspostSharp)"
    let createRequest (token: IDeviantArtAccessToken) (url: string) =
        let req = WebRequest.CreateHttp url
        req.UserAgent <- userAgent
        req.Headers.["Authorization"] <- sprintf "Bearer %s" token.AccessToken
        req
    let asyncRead (req: WebRequest) = async {
        try
            use! resp = req.AsyncGetResponse()
            use sr = new StreamReader(resp.GetResponseStream())
            let! json = sr.ReadToEndAsync() |> Async.AwaitTask
            let obj = DeviantArtBaseResponse.Parse json
            if obj.Status = "error" then
                let error_obj = DeviantArtErrorResponse.Parse json
                return raise (new DeviantArtException(resp, error_obj))
            else
                return json
        with
            | :? WebException as ex ->
                use resp = ex.Response
                use sr = new StreamReader(resp.GetResponseStream())
                let! json = sr.ReadToEndAsync() |> Async.AwaitTask
                let error_obj = DeviantArtErrorResponse.Parse json
                return raise (new DeviantArtException(resp, error_obj))
    }