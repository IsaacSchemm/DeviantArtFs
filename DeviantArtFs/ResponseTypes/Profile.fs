namespace DeviantArtFs.ResponseTypes

open DeviantArtFs
type Profile = {
    user: User
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
    last_status: Status option
    stats: ProfileStats
    collections: CollectionFolder list option
    galleries: GalleryFolder list option
}