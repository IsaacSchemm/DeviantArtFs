namespace DeviantArtFs

open System
open System.Net
open System.IO
open System.Threading.Tasks
open FSharp.Control

module internal dafs =
    let assertSuccess (resp: SuccessOrErrorResponse.Root) =
        match (resp.Success, resp.ErrorDescription) with
        | (true, None) -> ()
        | _ -> failwithf "%s" (resp.ErrorDescription |> Option.defaultValue "An unknown error occurred.")

    let urlEncode = WebUtility.UrlEncode
    let userAgent = "DeviantArtFs/0.5 (https://github.com/libertyernie/CrosspostSharp)"

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

    let parsePage (f: string -> 'a) (json: string) =
        let o = GenericListResponse.Parse json
        {
            HasMore = o.HasMore
            NextOffset = o.NextOffset
            HasLess = o.HasLess
            PrevOffset = o.PrevOffset
            EstimatedTotal = o.EstimatedTotal
            Name = o.Name
            Results = seq {
                for element in o.Results do
                    let json = element.JsonValue.ToString()
                    yield f json
            }
        }

    let parseListOnly (f: string -> 'a) (json: string) =
        let o = ListOnlyResponse.Parse json
        seq {
            for element in o.Results do
                let json = element.JsonValue.ToString()
                yield f json
        }

    let parseUser (json: string) =
        let u = UserResponse.Parse json
        {
            Userid = u.Userid
            Username = u.Username
            Usericon = u.Usericon
            Type = u.Type
        }

    let toPlainTask (t: Task<unit>) = t :> Task

    let toAsyncSeq (offset: int) (f: PagingParams -> Async<'a> when 'a :> IDeviantArtPagedResult<'b>) = asyncSeq {
        let mutable cursor = offset
        let mutable has_more = true
        while has_more do
            let! resp = new PagingParams(Offset = cursor, Limit = Nullable 24) |> f
            for r in resp.Results do
                yield r
            cursor <- resp.NextOffset.GetValueOrDefault()
            has_more <- resp.HasMore
    }