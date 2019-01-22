namespace DeviantArtFs

[<AllowNullLiteral>]
type IBclDeviationMetadataStats =
    abstract member Views: int
    abstract member ViewsToday: int
    abstract member Favourites: int
    abstract member Comments: int
    abstract member Downloads: int
    abstract member DownloadsToday: int

type DeviationMetadataStats = {
    views: int
    views_today: int
    favourites: int
    comments: int
    downloads: int
    downloads_today: int
} with
    interface IBclDeviationMetadataStats with
        member this.Comments = this.comments
        member this.Downloads = this.downloads
        member this.DownloadsToday = this.downloads_today
        member this.Favourites = this.favourites
        member this.Views = this.views
        member this.ViewsToday = this.views_today