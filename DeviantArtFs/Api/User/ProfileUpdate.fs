namespace DeviantArtFs.Api.User

open DeviantArtFs

type ArtistLevel =
| None=0
| Student=1
| Hobbyist=2
| Professional=3

type ArtistSpecialty =
| None=0
| ArtisanCrafts = 1
| DesignAndInterfaces = 2
| DigitalArt = 3
| FilmAndAnimation = 4
| Literature = 5
| Photography = 6
| TraditionalArt = 7
| Other = 8
| Varied = 9

[<RequireQualifiedAccess>]
type ProfileUpdateField =
| UserIsArtist of bool
| ArtistLevel of ArtistLevel
| ArtistSpecialty of ArtistSpecialty
| Tagline of string
| Countryid of int
| Website of string

module ProfileUpdate =
    let AsyncExecute token (updates: ProfileUpdateField seq) = async {
        let query = seq {
            for update in updates do
                match update with
                | ProfileUpdateField.UserIsArtist v -> sprintf "user_is_artist=%b" v
                | ProfileUpdateField.ArtistLevel v -> sprintf "artist_level=%s" (v.ToString "d")
                | ProfileUpdateField.ArtistSpecialty v -> sprintf "artist_specialty=%s" (v.ToString "d")
                | ProfileUpdateField.Tagline v -> sprintf "tagline=%s" (Dafs.urlEncode v)
                | ProfileUpdateField.Countryid v -> sprintf "countryid=%d" v
                | ProfileUpdateField.Website v -> sprintf "website=%s" (Dafs.urlEncode v)
        }
        let req = Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/user/profile/update" Seq.empty
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"
        req.RequestBodyText <- String.concat "&" query
        
        return! req
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>
    }

    let ExecuteAsync token updates =
        AsyncExecute token updates
        |> Async.StartAsTask