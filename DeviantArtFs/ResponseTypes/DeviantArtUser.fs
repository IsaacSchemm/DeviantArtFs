namespace DeviantArtFs

open System

type DeviantArtUserDetails = {
    sex: string option
    age: int option
    joindate: DateTimeOffset
}

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
}

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
}