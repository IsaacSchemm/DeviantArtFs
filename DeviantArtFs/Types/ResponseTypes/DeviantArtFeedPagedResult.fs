namespace DeviantArtFs

open FSharp.Json

type IBclDeviantArtFeedPagedResult =
    abstract member Cursor: string
    abstract member HasMore: bool
    abstract member Items: seq<IBclDeviantArtFeedItem>

type DeviantArtFeedPagedResult = {
    cursor: string
    has_more: bool
    items: DeviantArtFeedItem[]
} with
    static member Parse json = Json.deserialize<DeviantArtFeedPagedResult> json
    interface IBclDeviantArtFeedPagedResult with
        member this.Cursor = this.cursor
        member this.HasMore = this.has_more
        member this.Items = this.items |> Seq.map (fun o -> o :> IBclDeviantArtFeedItem)