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
    abstract member UserIsArtist: bool
    abstract member ArtistLevel: string
    abstract member ArtistSpecialty: string
    abstract member RealName: string
    abstract member Tagline: string
    abstract member Website: string
    abstract member CoverPhoto: string

type DeviantArtUserProfile = {
    user_is_artist: bool
    artist_level: string option
    artist_specialty: string option
    real_name: string
    tagline: string
    website: string
    cover_photo: string
    //profile_pic: Deviation
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
    abstract memberFriends: int

type DeviantArtUserStats = {
    watchers: int
    friends: int
} with
    interface IBclDeviantArtUserStats with
        member this.Watchers = this.watchers
        member this.memberFriends = this.friends

[<AllowNullLiteral>]
type IBclDeviantArtUser =
    abstract member Userid: Guid
    abstract member Username: string
    abstract member Usericon: string
    abstract member Type: string
    abstract member IsWatching: Nullable<bool>
    abstract member Details: IBclDeviantArtUserDetails
    abstract member Geo: IBclDeviantArtUserGeo
    abstract member Profile: IBclDeviantArtUserProfile
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