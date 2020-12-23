namespace DeviantArtFs.Api.User

open DeviantArtFs

type ProfileByNameRequest(username: string) =
    member __.Username = username
    member val ExtCollections = false with get, set
    member val ExtGalleries = false with get, set

module ProfileByName =
    let AsyncExecute token (req: ProfileByNameRequest) = async {
        let query = seq {
            yield sprintf "ext_collections=%b" req.ExtCollections
            yield sprintf "ext_galleries=%b" req.ExtGalleries
        }
        let qs = String.concat "&" query
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/user/profile/%s?%s" (Dafs.urlEncode req.Username) qs
            |> Dafs.createRequest token
        let! json = Dafs.asyncRead req
        return DeviantArtProfile.Parse json
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask