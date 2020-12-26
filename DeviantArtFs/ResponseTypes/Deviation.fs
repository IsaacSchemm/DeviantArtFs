namespace DeviantArtFs

open System
open FSharp.Json
open DeviantArtFs.Transforms

type DeviationStats = {
    comments: int
    favourites: int
}

type DeviationVideo = {
    src: string
    quality: string
    filesize: int
    duration: int
}

type DeviationFlash = {
    src: string
    height: int
    width: int
}

type DailyDeviation = {
    body: string
    time: DateTimeOffset
    giver: DeviantArtUser
    suggester: DeviantArtUser option
}

//type PremiumFolderData = {
//    ``type``: string
//    has_access: bool
//    gallery_id: Guid
//    points_price: int option
//    dollar_price: decimal option
//    num_subscribers: int option
//    subproductid: int option
//}

//type SuggestedReason =
//| WatchingUser = 1
//| SimilarToFavoritesOrHistory = 4
//| DailyDeviation = 5

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
    author: DeviantArtUser option
    stats: DeviationStats option
    [<JsonField(Transform=typeof<DateTimeOffsetEpochAsString>)>]
    published_time: DateTimeOffset option
    allows_comments: bool option
    preview: DeviationPreview option
    content: DeviationContent option
    thumbs: DeviationPreview list option
    videos: DeviationVideo list option
    flash: DeviationFlash option
    daily_deviation: DailyDeviation option
    //premium_folder_data: PremiumFolderData option
    //text_content: EditorText option
    excerpt: string option
    is_mature: bool option
    is_downloadable: bool option
    download_filesize: int option
    //suggested_reasons: SuggestedReason list option
}