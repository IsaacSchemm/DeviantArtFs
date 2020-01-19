namespace DeviantArtFs

type StashStats = {
    views: int option
    views_today: int option
    downloads: int option
    downloads_today: int option
} with
    member this.GetViewsOrNull() = this.views |> Option.toNullable
    member this.GetViewsTodayOrNull() = this.views_today |> Option.toNullable
    member this.GetDownloadsOrNull() = this.downloads |> Option.toNullable
    member this.GetDownloadsTodayOrNull() = this.downloads_today |> Option.toNullable