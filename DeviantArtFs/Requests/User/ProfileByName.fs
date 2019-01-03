namespace DeviantArtFs.Requests.User

open DeviantArtFs

type ProfileRequest(username: string) =
    member __.Username = username
    member val ExtCollections = false with get, set
    member val ExtGalleries = false with get, set

module ProfileByName =
    let AsyncExecute token (req: ProfileRequest) = async {
        let query = seq {
            yield sprintf "ext_collections=%b" req.ExtCollections
            yield sprintf "ext_galleries=%b" req.ExtGalleries
        }
        let qs = String.concat "&" query
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/user/profile/%s?%s" (dafs.urlEncode req.Username) qs
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return ProfileResponse.Parse json |> Profile
    }

    let AsyncGetUser token req = AsyncExecute token req |> dafs.whenDone (fun p -> p.User)
        
    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask
    let GetUserAsync token req = AsyncGetUser token req |> Async.StartAsTask