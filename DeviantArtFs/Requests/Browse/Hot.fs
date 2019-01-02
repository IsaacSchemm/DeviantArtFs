namespace DeviantArtFs.Requests.Browse

open DeviantArtFs

type HotRequest() = 
    member val CategoryPath = null with get, set
    member val Offset = 0 with get, set
    member val Limit = 10 with get, set

module Hot =
    let AsyncExecute token (req: HotRequest) = async {
        let query = seq {
            match Option.ofObj req.CategoryPath with
            | Some s -> yield sprintf "category_path=%s" (dafs.urlEncode s)
            | None -> ()
            yield sprintf "offset=%d" req.Offset
            yield sprintf "limit=%d" req.Limit
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/hot?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let o = GenericListResponse.Parse json
        return {
            HasMore = o.HasMore
            NextOffset = o.NextOffset
            Results = seq {
                for element in o.Results do
                    let json = element.JsonValue.ToString()
                    yield json |> DeviationResponse.Parse |> Deviation
            }
        }
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask