namespace DeviantArtFs.Api.User

open DeviantArtFs

type FriendsSearchRequest(query: string) =
    member __.Query = query
    member val Username = null with get, set

module FriendsSearch =
    let AsyncExecute token (req: FriendsSearchRequest) =
        seq {
            yield req.Query |> Dafs.urlEncode |> sprintf "query=%s"
            if req.Username |> isNull |> not then
                yield req.Username |> Dafs.urlEncode |> sprintf "username=%s"
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/user/friends/search"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtListOnlyResponse<DeviantArtUser>>

    let ExecuteAsync token req =
        AsyncExecute token req
        |> Async.StartAsTask