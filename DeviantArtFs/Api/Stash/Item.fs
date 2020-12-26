namespace DeviantArtFs.Api.Stash

open DeviantArtFs

type ItemRequest(itemid: int64) = 
    member __.Itemid = itemid
    member val ExtParams = DeviantArtExtParams.None with get, set

module Item =
    let AsyncExecute token (req: ItemRequest) =
        seq {
            yield! QueryFor.extParams req.ExtParams
        }
        |> Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/item/%d" req.Itemid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashMetadata>

    let ExecuteAsync token req =
        AsyncExecute token req
        |> Async.StartAsTask