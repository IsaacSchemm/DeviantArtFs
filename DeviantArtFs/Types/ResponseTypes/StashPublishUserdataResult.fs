namespace DeviantArtFs

open FSharp.Json

type IBclStashPublishUserdataResult =
    abstract member Features: seq<string>
    abstract member Agreements: seq<string>

type StashPublishUserdataResult = {
    features: string list
    agreements: string list
} with
    static member Parse json = Json.deserialize<StashPublishUserdataResult> json
    interface IBclStashPublishUserdataResult with
        member this.Features = this.features |> Seq.ofList
        member this.Agreements = this.agreements |> Seq.ofList