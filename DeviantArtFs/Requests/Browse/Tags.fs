namespace DeviantArtFs.Requests.Browse

open DeviantArtFs
open DeviantArtFs.Interop

type TagsRequest(tag: string) =
    member __.Tag = tag
    member val Offset = 0 with get, set
    member val Limit = 10 with get, set

module Tags =
    let AsyncExecute token (req: TagsRequest) = async {
        let query = seq {
            yield sprintf "tag=%s" (dafs.urlEncode req.Tag)
            yield sprintf "offset=%d" req.Offset
            yield sprintf "limit=%d" req.Limit
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/tags?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return dafs.parseSearchList DeviationResponse.Parse json
    }

    let ExecuteAsync token req = AsyncExecute token req |> iop.thenMapResult Deviation |> Async.StartAsTask