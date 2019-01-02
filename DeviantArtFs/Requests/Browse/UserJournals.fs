namespace DeviantArtFs.Requests.Browse

open DeviantArtFs

type UserJournalsRequest(username: string) =
    member __.Username = username
    member val Featured = true with get, set
    member val Offset = 0 with get, set
    member val Limit = 10 with get, set

module UserJournals =
    let AsyncExecute token (req: UserJournalsRequest) = async {
        let query = seq {
            yield sprintf "username=%s" (dafs.urlEncode req.Username)
            yield sprintf "featured=%b" req.Featured
            yield sprintf "offset=%d" req.Offset
            yield sprintf "limit=%d" req.Limit
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/user/journals?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let o = GenericListResponse.Parse json
        return {
            HasMore = o.HasMore
            NextOffset = o.NextOffset
            Results = seq {
                for element in o.Results do
                    let json = element.JsonValue.ToString()
                    yield DeviationResponse.Parse json
            }
        }
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask