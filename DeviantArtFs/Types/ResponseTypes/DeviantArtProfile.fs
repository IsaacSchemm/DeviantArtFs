namespace DeviantArtFs

open DeviantArtFs
open FSharp.Json

[<AllowNullLiteral>]
type IBclDeviantArtProfile =
    inherit IBclDeviantArtUserProfile

    /// Information about the user.
    abstract member User: IBclDeviantArtUser
    /// Whether the logged-in user is watching this user.
    abstract member IsWatching: bool
    /// The user's profile URL.
    abstract member ProfileUrl: string
    /// An ID representing the user's country.
    abstract member Countryid: int
    /// The name of the user's country.
    abstract member Country: string
    /// A short HTML bio for the user.
    abstract member Bio: string
    /// The deviation that the user has selected as their profile pic. May be null.
    abstract member ProfilePic: IBclDeviation
    /// The last status posted by this user. May be null.
    abstract member LastStatus: IBclDeviantArtStatus
    /// Statistics such as number of deviations and number of pageviews. May be null.
    abstract member Stats: IBclDeviantArtProfileStats
    /// A list of the user's collections. May be empty.
    abstract member Collections: seq<IBclDeviantArtCollectionFolder>
    /// A list of the user's galleries. May be empty.
    abstract member Galleries: seq<IBclDeviantArtGalleryFolder>

type DeviantArtProfile = {
    user: DeviantArtUser
    is_watching: bool
    profile_url: string
    user_is_artist: bool
    artist_level: string option
    artist_specialty: string option
    real_name: string
    tagline: string
    countryid: int
    country: string
    website: string
    bio: string
    cover_photo: string option
    profile_pic: Deviation option
    last_status: DeviantArtStatus option
    stats: DeviantArtProfileStats
    collections: DeviantArtCollectionFolder list option
    galleries: DeviantArtGalleryFolder list option
} with
    static member Parse json = Json.deserialize<DeviantArtProfile> json
    interface IBclDeviantArtProfile with
        member this.ArtistLevel = this.artist_level |> Option.toObj
        member this.ArtistSpecialty = this.artist_specialty |> Option.toObj
        member this.Bio = this.bio
        member this.Collections =
            this.collections
            |> Option.defaultValue List.empty
            |> Seq.map (fun d -> d :> IBclDeviantArtCollectionFolder)
        member this.Country = this.country
        member this.Countryid = this.countryid
        member this.CoverPhoto = this.cover_photo |> Option.toObj
        member this.Galleries =
            this.galleries
            |> Option.defaultValue List.empty
            |> Seq.map (fun d -> d :> IBclDeviantArtGalleryFolder)
        member this.IsWatching = this.is_watching
        member this.LastStatus = this.last_status |> Option.map (fun s -> s :> IBclDeviantArtStatus) |> Option.toObj
        member this.ProfilePic = this.profile_pic |> Option.map (fun o -> o :> IBclDeviation) |> Option.toObj
        member this.ProfileUrl = this.profile_url
        member this.RealName = this.real_name
        member this.Stats = this.stats :> IBclDeviantArtProfileStats
        member this.Tagline = this.tagline
        member this.User = this.user :> IBclDeviantArtUser
        member this.UserIsArtist = this.user_is_artist
        member this.Website = this.website