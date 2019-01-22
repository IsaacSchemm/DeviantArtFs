namespace DeviantArtFs

open System
open FSharp.Json

type IBclStashPublishResponse =
    abstract member Status: string
    abstract member Url: string
    abstract member Deviationid: Guid

type StashPublishResponse = {
    status: string
    url: string
    deviationid: Guid
} with
    static member Parse json = Json.deserialize<StashPublishResponse> json
    interface IBclStashPublishResponse with
        member this.Status = this.status
        member this.Url = this.url
        member this.Deviationid = this.deviationid