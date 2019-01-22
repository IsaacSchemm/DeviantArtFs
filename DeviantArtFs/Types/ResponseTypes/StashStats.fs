namespace DeviantArtFs

open System
    
[<AllowNullLiteral>]
type IBclStashStats =
    abstract member Views: Nullable<int>
    abstract member ViewsToday: Nullable<int>
    abstract member Downloads: Nullable<int>
    abstract member DownloadsToday: Nullable<int>

type StashStats = {
    views: int option
    views_today: int option
    downloads: int option
    downloads_today: int option
} with
    interface IBclStashStats with
        member this.Views = this.views |> Option.toNullable
        member this.ViewsToday = this.views_today |> Option.toNullable
        member this.Downloads = this.downloads |> Option.toNullable
        member this.DownloadsToday = this.downloads_today |> Option.toNullable