namespace DeviantArtFs.Api.User

open DeviantArtFs

type ProfileByNameRequest(username: string) =
    member __.Username = username
    member val ExtCollections = false with get, set
    member val ExtGalleries = false with get, set

module ProfileByName =
    let AsyncExecute token expansion (req: ProfileByNameRequest) =
        seq {
            yield sprintf "ext_collections=%b" req.ExtCollections
            yield sprintf "ext_galleries=%b" req.ExtGalleries
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/profile/%s" (Dafs.urlEncode req.Username))
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtProfile>

    let ExecuteAsync token expansion req =
        AsyncExecute token expansion req
        |> Async.StartAsTask