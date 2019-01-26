namespace DeviantArtFs.Requests.Stash

open DeviantArtFs

type ItemRequest(itemid: int64) = 
    member __.Itemid = itemid
    member val ExtParams = DeviantArtExtParams.None with get, set

module Item =
    let AsyncExecute token (req: ItemRequest) = async {
        let query = seq {
            yield! QueryFor.extParams req.ExtParams
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/stash/item/%d?%s" req.Itemid
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return StashMetadata.Parse json
    }

    let ExecuteAsync token req = AsyncExecute token req |> AsyncThen.map (fun i -> i :> IBclStashMetadata) |> Async.StartAsTask