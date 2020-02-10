namespace DeviantArtFs

type StashStats = {
    views: int option
    views_today: int option
    downloads: int option
    downloads_today: int option
} with
    member this.GetViews() = this.views |> OptUtils.intDefault
    member this.GetViewsToday() = this.views_today |> OptUtils.intDefault
    member this.GetDownloads() = this.downloads |> OptUtils.intDefault
    member this.GetDownloadsToday() = this.downloads_today |> OptUtils.intDefault