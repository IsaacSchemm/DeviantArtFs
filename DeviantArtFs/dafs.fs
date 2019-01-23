namespace DeviantArtFs

open System
open System.Net
open System.IO
open System.Threading.Tasks
open FSharp.Control

module internal dafs =
    let assertSuccess (resp: SuccessOrErrorResponse) =
        match (resp.success, resp.error_description) with
        | (true, None) -> ()
        | _ -> failwithf "%s" (resp.error_description |> Option.defaultValue "An unknown error occurred.")

    let urlEncode = WebUtility.UrlEncode
    let userAgent = "DeviantArtFs/0.6 (https://github.com/libertyernie/DeviantArtFs)"

    let createRequest (token: IDeviantArtAccessToken) (url: string) =
        let req = WebRequest.CreateHttp url
        req.UserAgent <- userAgent
        req.Headers.["Authorization"] <- sprintf "Bearer %s" token.AccessToken
        req

    let mutable retry429 = 500

    let rec asyncRead (req: WebRequest) = async {
        try
            use! resp = req.AsyncGetResponse()
            use sr = new StreamReader(resp.GetResponseStream())
            let! json = sr.ReadToEndAsync() |> Async.AwaitTask
            let obj = DeviantArtBaseResponse.Parse json
            if obj.status = Some "error" then
                return raise (new DeviantArtException(resp, obj))
            else
                retry429 <- 500
                return json
        with
            | :? WebException as ex ->
                use resp = ex.Response :?> HttpWebResponse
                if int resp.StatusCode = 429 then
                    retry429 <- Math.Max(retry429 * 2, 60000)
                    if retry429 >= 60000 then
                        return failwithf "Client is rate-limited (too many 429 responses)"
                    do! Async.Sleep retry429
                    return! asyncRead req
                else
                    use sr = new StreamReader(resp.GetResponseStream())
                    let! json = sr.ReadToEndAsync() |> Async.AwaitTask
                    let error_obj = DeviantArtBaseResponse.Parse json
                    return raise (new DeviantArtException(resp, error_obj))
    }

    let parseUser (json: string) = DeviantArtUser.Parse json

    let toPlainTask (t: Task<unit>) = t :> Task

    let toAsyncSeq (offset: int) (limit: int) (req: 'a) (f: PagingParams -> 'a -> Async<'b> when 'b :> IDeviantArtConvertibleToAsyncSeq<'c>) = asyncSeq {
        let mutable cursor = offset
        let mutable has_more = true
        while has_more do
            let paging = new PagingParams(Offset = cursor, Limit = Nullable limit)
            let! resp = f paging req
            for r in resp.Results do
                yield r
            cursor <- resp.NextOffset |> Option.defaultValue 0
            has_more <- resp.HasMore
    }

    let cursorToAsyncSeq (initial_cursor: string option) (f: string option -> Async<DeviantArtCursorResult<'c>>) = asyncSeq {
        let mutable cursor = initial_cursor
        let mutable has_more = true
        while has_more do
            let! resp = f cursor
            for r in resp.items do
                yield r
            cursor <- Some resp.cursor
            has_more <- resp.has_more
    }

    let asBclUser u = u :> IBclDeviantArtUser