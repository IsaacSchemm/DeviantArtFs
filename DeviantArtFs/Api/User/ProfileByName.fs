namespace DeviantArtFs.Api.User

open DeviantArtFs

type ProfileByNameRequest(username: string) =
    member __.Username = username
    member val ExtCollections = false with get, set
    member val ExtGalleries = false with get, set

module ProfileByName =
    let AsyncExecute token common (req: ProfileByNameRequest) =
        seq {
            yield sprintf "ext_collections=%b" req.ExtCollections
            yield sprintf "ext_galleries=%b" req.ExtGalleries
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/profile/%s" (Dafs.urlEncode req.Username))
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtProfile>

    let ExecuteAsync token common req =
        AsyncExecute token common req
        |> Async.StartAsTask