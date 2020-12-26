namespace DeviantArtFs.Api.Stash

open DeviantArtFs

type ItemRequest(itemid: int64) = 
    member __.Itemid = itemid
    member val ExtParams = DeviantArtExtParams.None with get, set

module Item =
    let AsyncExecute token common (req: ItemRequest) =
        seq {
            yield! QueryFor.extParams req.ExtParams
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/item/%d" req.Itemid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashMetadata>

    let ExecuteAsync token common req =
        AsyncExecute token common req
        |> Async.StartAsTask