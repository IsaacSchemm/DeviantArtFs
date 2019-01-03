namespace DeviantArtFs.Requests.User

open DeviantArtFs
open System

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
    member val UserIsArtist = Nullable<bool>() with get, set
    member val ArtistLevel = Nullable<ArtistLevel>() with get, set
    member val ArtistSpecialty = Nullable<ArtistSpecialty>() with get, set
    member val RealName = null with get, set
    member val Tagline = null with get, set
    member val Countryid = Nullable<int>() with get, set
    member val Website = null with get, set
    member val Bio = null with get, set

module ProfileUpdate =
    open System.IO

    let AsyncExecute token (ps: ProfileUpdateRequest) = async {
        let query = seq {
            match Option.ofNullable ps.UserIsArtist with
            | Some s -> yield sprintf "user_is_artist=%b" s
            | None -> ()
            match Option.ofNullable ps.ArtistLevel with
            | Some s -> yield sprintf "artist_level=%O" (s.ToString("d"))
            | None -> ()
            match Option.ofNullable ps.ArtistSpecialty with
            | Some s -> yield sprintf "artist_specialty=%O" (s.ToString("d"))
            | None -> ()
            match Option.ofObj ps.RealName with
            | Some s -> yield dafs.urlEncode s |> sprintf "real_name=%s"
            | None -> ()
            match Option.ofObj ps.Tagline with
            | Some s -> yield  dafs.urlEncode s |> sprintf "tagline=%s"
            | None -> ()
            match Option.ofNullable ps.Countryid with
            | Some s -> yield sprintf "countryid=%d" s
            | None -> ()
            match Option.ofObj ps.Website with
            | Some s -> yield  dafs.urlEncode s |> sprintf "website=%s"
            | None -> ()
            match Option.ofObj ps.Bio with
            | Some s -> yield  dafs.urlEncode s |> sprintf "real_name=%s"
            | None -> ()
        }
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/user/profile/update"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"
        do! async {
            use! stream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(stream)
            do! String.concat "&" query |> sw.WriteAsync |> Async.AwaitTask
        }
        let! json = dafs.asyncRead req
        SuccessOrErrorResponse.Parse json |> dafs.assertSuccess
    }

    let ExecuteAsync token ps = AsyncExecute token ps |> Async.StartAsTask