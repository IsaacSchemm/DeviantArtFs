namespace DeviantArtFs

open DeviantArtFs
open FSharp.Json

type IBclDeviantArtProfile =
    abstract member User: IDeviantArtUser
    abstract member IsWatching: bool
    abstract member ProfileUrl: string
    abstract member UserIsArtist: bool
    abstract member ArtistLevel: string
    abstract member ArtistSpecialty: string
    abstract member RealName: string
    abstract member Tagline: string
    abstract member Countryid: int
    abstract member Country: string
    abstract member Website: string
    abstract member Bio: string
    abstract member CoverPhoto: string
    abstract member ProfilePic: IBclDeviation
    abstract member LastStatus: IBclDeviantArtStatus
    abstract member Stats: IBclDeviantArtProfileStats
    abstract member Collections: seq<IBclDeviantArtCollectionFolder>
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
    collections: DeviantArtCollectionFolder[] option
    galleries: DeviantArtGalleryFolder[] option
} with
    static member Parse json = Json.deserialize<DeviantArtProfile> json
    interface IBclDeviantArtProfile with
        member this.ArtistLevel = this.artist_level |> Option.toObj
        member this.ArtistSpecialty = this.artist_specialty |> Option.toObj
        member this.Bio = this.bio
        member this.Collections =
            this.collections
            |> Option.map Seq.ofArray
            |> Option.defaultValue Seq.empty
            |> Seq.map (fun d -> d :> IBclDeviantArtCollectionFolder)
        member this.Country = this.country
        member this.Countryid = this.countryid
        member this.CoverPhoto = this.cover_photo |> Option.toObj
        member this.Galleries =
            this.galleries
            |> Option.map Seq.ofArray
            |> Option.defaultValue Seq.empty
            |> Seq.map (fun d -> d :> IBclDeviantArtGalleryFolder)
        member this.IsWatching = this.is_watching
        member this.LastStatus = this.last_status |> Option.map (fun s -> s :> IBclDeviantArtStatus) |> Option.toObj
        member this.ProfilePic = this.profile_pic |> Option.map (fun d -> d :> IBclDeviation) |> Option.toObj
        member this.ProfileUrl = this.profile_url
        member this.RealName = this.real_name
        member this.Stats = this.stats :> IBclDeviantArtProfileStats
        member this.Tagline = this.tagline
        member this.User = this.user :> IDeviantArtUser
        member this.UserIsArtist = this.user_is_artist
        member this.Website = this.website