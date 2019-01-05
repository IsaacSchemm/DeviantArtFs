namespace DeviantArtFs

open System.Net
open System.IO
open FSharp.Data

module internal dafs =
    let assertSuccess (resp: SuccessOrErrorResponse.Root) =
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

    let parseGenericList (f: string -> 'a) (json: string) =
        let o = GenericListResponse.Parse json
        {
            HasMore = o.HasMore
            NextOffset = o.NextOffset
            Results = seq {
                for element in o.Results do
                    let json = element.JsonValue.ToString()
                    yield f json
            }
        }

    let parseSearchList (f: string -> 'a) (json: string) =
        let o = SearchListResponse.Parse json
        {
            HasMore = o.HasMore
            NextOffset = o.NextOffset
            EstimatedTotal = o.EstimatedTotal
            Results = seq {
                for element in o.Results do
                    let json = element.JsonValue.ToString()
                    yield f json
            }
        }

    let parseBidirectionalList (f: string -> 'a) (json: string) =
        let o = BidirectionalListResponse.Parse json
        {
            HasMore = o.HasMore
            NextOffset = o.NextOffset
            HasLess = o.HasLess
            PrevOffset = o.PrevOffset
            Results = seq {
                for element in o.Results do
                    let json = element.JsonValue.ToString()
                    yield f json
            }
        }

    let parseGalleryList (f: string -> 'a) (json: string) =
        let o = GalleryListResponse.Parse json
        {
            HasMore = o.HasMore
            NextOffset = o.NextOffset
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