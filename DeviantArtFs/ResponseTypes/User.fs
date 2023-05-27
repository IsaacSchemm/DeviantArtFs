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

type UserLinkSubnav = {
    content_type: string
}

type UserSidebarWatched = {
    has_new_content: bool
    link_subnav: UserLinkSubnav
    is_pinned: bool
}

type UserSidebar = {
    watched: UserSidebarWatched option
}

type User = {
    userid: Guid
    username: string
    usericon: string
    ``type``: string
    is_watching: bool option
    is_subscribed: bool option
    details: UserDetails option
    geo: UserGeo option
    profile: UserProfile option
    stats: UserStats option
    sidebar: UserSidebar option
}