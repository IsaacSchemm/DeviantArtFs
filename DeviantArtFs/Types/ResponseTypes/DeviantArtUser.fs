namespace DeviantArtFs

open System
open FSharp.Json

[<AllowNullLiteral>]
type IBclDeviantArtUserDetails =
    abstract member Sex: string
    abstract member Age: Nullable<int>
    abstract member Joindate: DateTimeOffset

type DeviantArtUserDetails = {
    sex: string option
    age: int option
    joindate: DateTimeOffset
} with
    interface IBclDeviantArtUserDetails with
        member this.Age = this.age |> Option.toNullable
        member this.Joindate = this.joindate
        member this.Sex = this.sex |> Option.toObj

[<AllowNullLiteral>]
type IBclDeviantArtUserGeo =
    abstract member Country: string
    abstract member Countryid: int
    abstract member Timezone: string

type DeviantArtUserGeo = {
    country: string
    countryid: int
    timezone: string
} with
    interface IBclDeviantArtUserGeo with
        member this.Country = this.country
        member this.Countryid = this.countryid
        member this.Timezone = this.timezone

[<AllowNullLiteral>]
type IBclDeviantArtUserProfile =
    /// Whether the user is an artist.
    abstract member UserIsArtist: bool
    /// The user's artist level, as displayed on their profile page.
    abstract member ArtistLevel: string
    /// The user's specialty, as displayed on their profile page.
    abstract member ArtistSpecialty: string
    /// The user's real name, as displayed on their profile page.
    abstract member RealName: string
    /// The user's tagline, as displayed on their profile page.
    abstract member Tagline: string
    /// The user's website URL.
    abstract member Website: string
    /// The user's cover photo URL. May be null.
    abstract member CoverPhoto: string

type DeviantArtUserProfile = {
    user_is_artist: bool
    artist_level: string option
    artist_specialty: string option
    real_name: string
    tagline: string
    website: string
    cover_photo: string
} with
    interface IBclDeviantArtUserProfile with
        member this.ArtistLevel = this.artist_level |> Option.toObj
        member this.ArtistSpecialty = this.artist_specialty |> Option.toObj
        member this.CoverPhoto = this.cover_photo
        member this.RealName = this.real_name
        member this.Tagline = this.tagline
        member this.UserIsArtist = this.user_is_artist
        member this.Website = this.website

[<AllowNullLiteral>]
type IBclDeviantArtUserStats =
    abstract member Watchers: int
    abstract member Friends: int

type DeviantArtUserStats = {
    watchers: int
    friends: int
} with
    interface IBclDeviantArtUserStats with
        member this.Watchers = this.watchers
        member this.Friends = this.friends

[<AllowNullLiteral>]
type IBclDeviantArtUser =
    /// The user's ID in the DeviantArt API.
    abstract member Userid: Guid
    /// The username.
    abstract member Username: string
    /// A URL to the user's avatar.
    abstract member Usericon: string
    /// The type of user (e.g. "regular", "premium", "admin")
    abstract member Type: string

    /// Whether this user is watching the logged-in user. Not available in all requests.
    abstract member IsWatching: Nullable<bool>
    /// User details. May be null.
    abstract member Details: IBclDeviantArtUserDetails
    /// User location. May be null.
    abstract member Geo: IBclDeviantArtUserGeo
    // User profile info. May be null.
    abstract member Profile: IBclDeviantArtUserProfile
    // User statistics. May be null.
    abstract member Stats: IBclDeviantArtUserStats

type DeviantArtUser = {
    userid: Guid
    username: string
    usericon: string
    ``type``: string
    is_watching: bool option
    details: DeviantArtUserDetails option
    geo: DeviantArtUserGeo option
    profile: DeviantArtUserProfile option
    stats: DeviantArtUserStats option
} with
    static member Parse json = Json.deserialize<DeviantArtUser> json
    interface IBclDeviantArtUser with
        member this.Userid = this.userid
        member this.Username = this.username
        member this.Usericon = this.usericon
        member this.Type = this.``type``
        member this.IsWatching = this.is_watching |> Option.toNullable
        member this.Details = this.details |> Option.map (fun o -> o :> IBclDeviantArtUserDetails) |> Option.toObj
        member this.Geo = this.geo |> Option.map (fun o -> o :> IBclDeviantArtUserGeo) |> Option.toObj
        member this.Profile = this.profile |> Option.map (fun o -> o :> IBclDeviantArtUserProfile) |> Option.toObj
        member this.Stats = this.stats |> Option.map (fun o -> o :> IBclDeviantArtUserStats) |> Option.toObj