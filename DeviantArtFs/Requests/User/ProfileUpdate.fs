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
    member val UserIsArtist = DeviantArtFieldChange<bool>.NoChange with get, set
    member val ArtistLevel = DeviantArtFieldChange<ArtistLevel>.NoChange with get, set
    member val ArtistSpecialty = DeviantArtFieldChange<ArtistSpecialty>.NoChange with get, set
    member val RealName = DeviantArtFieldChange<string>.NoChange with get, set
    member val Tagline = DeviantArtFieldChange<string>.NoChange with get, set
    member val Countryid = DeviantArtFieldChange<int>.NoChange with get, set
    member val Website = DeviantArtFieldChange<string>.NoChange with get, set
    member val Bio = DeviantArtFieldChange<string>.NoChange with get, set

module ProfileUpdate =
    open System.IO

    let AsyncExecute token (ps: ProfileUpdateRequest) = async {
        let query = seq {
            yield! ps.UserIsArtist |> QueryFor.fieldChange "user_is_artist"
            yield! ps.ArtistLevel |> fch.map (fun s -> s.ToString("d")) |> QueryFor.fieldChange "artist_level"
            yield! ps.ArtistSpecialty |> fch.map (fun s -> s.ToString("d")) |> QueryFor.fieldChange "artist_specialty"
            yield! ps.RealName |> QueryFor.fieldChange "real_name"
            yield! ps.Tagline |> QueryFor.fieldChange "tagline"
            yield! ps.Countryid |> QueryFor.fieldChange "countryid"
            yield! ps.Website |> QueryFor.fieldChange "website"
            yield! ps.Bio |> QueryFor.fieldChange "bio"
        }
        let req = Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/user/profile/update"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"
        do! async {
            use! stream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(stream)
            String.concat "&" query |> printf "%s"
            do! String.concat "&" query |> sw.WriteAsync |> Async.AwaitTask
        }
        let! json = Dafs.asyncRead req
        DeviantArtSuccessOrErrorResponse.Parse json |> Dafs.assertSuccess
    }

    let ExecuteAsync token ps = AsyncExecute token ps |> Async.StartAsTask |> Dafs.toPlainTask