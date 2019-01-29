namespace DeviantArtFs.Requests.User

open DeviantArtFs

type FriendsSearchRequest(query: string) =
    member __.Query = query
    member val Username = null with get, set

module FriendsSearch =
    let AsyncExecute token (req: FriendsSearchRequest) = async {
        let query = seq {
            yield req.Query |> Dafs.urlEncode |> sprintf "query=%s"
            if req.Username |> isNull |> not then
                yield req.Username |> Dafs.urlEncode |> sprintf "username=%s"
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/search?%s"
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtListOnlyResponse<DeviantArtUser>.ParseSeq json
    }

    let ExecuteAsync token req = AsyncExecute token req |> AsyncThen.mapSeq (fun o -> o :> IBclDeviantArtUser) |> Async.StartAsTask