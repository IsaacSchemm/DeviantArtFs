namespace DeviantArtFs

open DeviantArtFs
open FSharp.Json

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
    collections: DeviantArtFolder list option
    galleries: DeviantArtFolder list option
} with
    static member Parse json = Json.deserialize<DeviantArtProfile> json
    member this.GetArtistLevel() = OptUtils.stringDefault this.artist_level
    member this.GetArtistSpecialty() = OptUtils.stringDefault this.artist_specialty
    member this.CoverPhoto() = OptUtils.stringDefault this.cover_photo
    member this.GetProfilePic() = OptUtils.recordDefault this.profile_pic
    member this.GetLastStatus() = OptUtils.recordDefault this.last_status
    member this.GetCollections() = OptUtils.listDefault this.collections
    member this.GetGalleries() = OptUtils.listDefault this.galleries