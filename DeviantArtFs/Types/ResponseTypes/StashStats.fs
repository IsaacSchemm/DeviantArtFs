namespace DeviantArtFs

type StashStats = {
    views: int option
    views_today: int option
    downloads: int option
    downloads_today: int option
} with
    member this.GetViews() = this.views |> OptUtils.toNullable
    member this.GetViewsToday() = this.views_today |> OptUtils.toNullable
    member this.GetDownloads() = this.downloads |> OptUtils.toNullable
    member this.GetDownloadsToday() = this.downloads_today |> OptUtils.toNullable