namespace DeviantArtFs

open DeviantArtFs
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
}