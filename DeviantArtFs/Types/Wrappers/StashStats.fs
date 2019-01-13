namespace DeviantArtFs

open System
    
[<AllowNullLiteral>]
type IBclStashStats =
    abstract member Views: Nullable<int>
    abstract member ViewsToday: Nullable<int>
    abstract member Downloads: Nullable<int>
    abstract member DownloadsToday: Nullable<int>

type StashStats(original: StashMetadataResponse.Stats) =
    member __.Original = original

    member __.Views = original.Views
    member __.ViewsToday = original.ViewsToday
    member __.Downloads = original.Downloads
    member __.DownloadsToday = original.DownloadsToday

    interface IBclStashStats with
        member this.Views = this.Views |> Option.toNullable
        member this.ViewsToday = this.ViewsToday |> Option.toNullable
        member this.Downloads = this.Downloads |> Option.toNullable
        member this.DownloadsToday = this.DownloadsToday |> Option.toNullable