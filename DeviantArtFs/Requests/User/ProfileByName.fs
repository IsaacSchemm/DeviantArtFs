namespace DeviantArtFs.Requests.User

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
            sprintf "https://www.deviantart.com/api/v1/oauth2/user/profile/%s?%s" (dafs.urlEncode req.Username) qs
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return ProfileResponse.Parse json |> DeviantArtProfile
    }

    let ExecuteAsync token req = AsyncExecute token req |> iop.thenTo (fun p -> p :> IBclDeviantArtProfile) |> Async.StartAsTask