namespace DeviantArtFs.ResponseTypes

open System
open FSharp.Json
open DeviantArtFs.Transforms

type Stats = {
    comments: int
    favourites: int
}

type Video = {
    src: string
    quality: string
    filesize: int
    duration: int
}

type Flash = {
    src: string
    height: int
    width: int
}

type DailyDeviation = {
    body: string
    time: DateTimeOffset
    giver: User
    suggester: User option
}

type SuggestedReason =
| WatchingUser = 1
| SimilarToRecentEngagement = 4

type TierSettings = {
    access_settings: string
}

type TierStats = {
    subscribers: int option
    deviations: int option
    posts: int option
    total: int option
}

type Tier = {
    state: string option
    is_user_subscribed: bool option
    can_user_subscribe: bool option
    subproductid: int option
    dollar_price: string option
    settings: TierSettings option
    stats: TierStats option
}

type PremiumFolderData = {
    ``type``: string
    has_access: bool
    gallery_id: Guid
    points_price: int option
    dollar_price: decimal option
    num_subscribers: int option
    subproductid: int option
}

type EditorBody = {
    ``type``: string
    markup: string option
    features: string
}

type EditorText = {
    excerpt: string
    body: EditorBody
}

type Content = {
    src: string
    height: int
    width: int
    transparency: bool
    filesize: int
}

type Deviation = {
    deviationid: Guid
    printid: Guid option
    url: string option
    title: string option
    category: string option
    category_path: string option
    is_favourited: bool option
    is_deleted: bool
    is_published: bool option
    author: User option
    stats: Stats option
    [<JsonField(Transform=typeof<DateTimeOffsetEpochAsString>)>] published_time: DateTimeOffset option
    allows_comments: bool option
    tier: Tier option
    preview: Preview option
    content: Content option
    thumbs: Preview list option
    videos: Video list option
    flash: Flash option
    daily_deviation: DailyDeviation option
    premium_folder_data: PremiumFolderData option
    text_context: EditorText option
    is_pinned: bool option
    cover_image: Deviation option
    tier_access: string option
    primary_tier: Deviation option
    excerpt: string option
    is_mature: bool option
    is_downloadable: bool option
    download_filesize: int option
    suggested_reasons: SuggestedReason list option
}