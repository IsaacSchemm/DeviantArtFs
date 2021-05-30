namespace DeviantArtFs.ResponseTypes

open System

type UserDetails = {
    sex: string option
    age: int option
    joindate: DateTimeOffset
}

type UserGeo = {
    country: string
    countryid: int
    timezone: string
}

type UserProfile = {
    user_is_artist: bool
    artist_level: string option
    artist_specialty: string option
    real_name: string
    tagline: string
    website: string
    cover_photo: string
}

type UserStats = {
    watchers: int
    friends: int
}

type User = {
    userid: Guid
    username: string
    usericon: string
    ``type``: string
    is_watching: bool option
    details: UserDetails option
    geo: UserGeo option
    profile: UserProfile option
    stats: UserStats option
}