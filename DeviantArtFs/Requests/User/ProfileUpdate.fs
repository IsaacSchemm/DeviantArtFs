namespace DeviantArtFs.Requests.User

open DeviantArtFs

type ArtistLevel =
    | Student=1
    | Hobbyist=2
    | Professional=3
type ArtistSpecialty =
    | ArtisanCrafts = 1
    | DesignAndInterfaces = 2
    | DigitalArt = 3
    | FilmAndAnimation = 4
    | Literature = 5
    | Photography = 6
    | TraditionalArt = 7
    | Other = 8
    | Varied = 9

type ProfileUpdateRequest() =
    member val UserIsArtist = FieldChange<bool>.NoChange with get, set
    member val ArtistLevel = FieldChange<ArtistLevel>.NoChange with get, set
    member val ArtistSpecialty = FieldChange<ArtistSpecialty>.NoChange with get, set
    member val RealName = FieldChange<string>.NoChange with get, set
    member val Tagline = FieldChange<string>.NoChange with get, set
    member val Countryid = FieldChange<int>.NoChange with get, set
    member val Website = FieldChange<string>.NoChange with get, set
    member val Bio = FieldChange<string>.NoChange with get, set

module ProfileUpdate =
    open System.IO

    let AsyncExecute token (ps: ProfileUpdateRequest) = async {
        let query = seq {
            yield! ps.UserIsArtist |> queryFor.fieldChange "user_is_artist"
            yield! ps.ArtistLevel |> fch.map (fun s -> s.ToString("d")) |> queryFor.fieldChange "artist_level"
            yield! ps.ArtistSpecialty |> fch.map (fun s -> s.ToString("d")) |> queryFor.fieldChange "artist_specialty"
            yield! ps.RealName |> queryFor.fieldChange "real_name"
            yield! ps.Tagline |> queryFor.fieldChange "tagline"
            yield! ps.Countryid |> queryFor.fieldChange "countryid"
            yield! ps.Website |> queryFor.fieldChange "website"
            yield! ps.Bio |> queryFor.fieldChange "bio"
        }
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/user/profile/update"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"
        do! async {
            use! stream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(stream)
            String.concat "&" query |> printf "%s"
            do! String.concat "&" query |> sw.WriteAsync |> Async.AwaitTask
        }
        let! json = dafs.asyncRead req
        SuccessOrErrorResponse.Parse json |> dafs.assertSuccess
    }

    let ExecuteAsync token ps = AsyncExecute token ps |> Async.StartAsTask |> dafs.toPlainTask