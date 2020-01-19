namespace DeviantArtFs

open System
open FSharp.Json

type DeviantArtUserDetails = {
    sex: string option
    age: int option
    joindate: DateTimeOffset
} with
    member this.GetAge() = OptUtils.toNullable this.age
    member this.GetSex() = OptUtils.stringDefault this.sex

type DeviantArtUserGeo = {
    country: string
    countryid: int
    timezone: string
}

type DeviantArtUserProfile = {
    user_is_artist: bool
    artist_level: string option
    artist_specialty: string option
    real_name: string
    tagline: string
    website: string
    cover_photo: string
} with
    member this.GetArtistLevel() = OptUtils.stringDefault this.artist_level
    member this.GetArtistSpecialty() = OptUtils.stringDefault this.artist_specialty

type DeviantArtUserStats = {
    watchers: int
    friends: int
}

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
    member this.GetIsWatching() = OptUtils.toNullable this.is_watching
    member this.GetDetails() = OptUtils.toSeq this.details
    member this.GetGeo() = OptUtils.toSeq this.geo
    member this.GetProfile() = OptUtils.toSeq this.profile
    member this.GetStats() = OptUtils.toSeq this.stats