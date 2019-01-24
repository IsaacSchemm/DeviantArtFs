namespace DeviantArtFs

open FSharp.Json

type IBclDeviantArtFeedInclude =
    abstract member Statuses: bool
    abstract member Deviations: bool
    abstract member Journals: bool
    abstract member GroupDeviations: bool
    abstract member Collections: bool
    abstract member Misc: bool

type IBclDeviantArtFeedSettings =
    abstract member Include: IBclDeviantArtFeedInclude

type DeviantArtFeedInclude = {
    statuses: bool
    deviations: bool
    journals: bool
    group_deviations: bool
    collections: bool
    misc: bool
} with
    interface IBclDeviantArtFeedInclude with
        member this.Collections = this.collections
        member this.Deviations = this.deviations
        member this.GroupDeviations = this.group_deviations
        member this.Journals = this.journals
        member this.Misc = this.misc
        member this.Statuses = this.statuses

type DeviantArtFeedSettings = {
    ``include``: DeviantArtFeedInclude
} with
    static member Parse (json: string) = Json.deserialize<DeviantArtFeedSettings> json
    interface IBclDeviantArtFeedSettings with
        member this.Include = this.``include`` :> IBclDeviantArtFeedInclude