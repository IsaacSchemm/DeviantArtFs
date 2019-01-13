namespace DeviantArtFs

type IBclStashMoveResult =
    abstract member Target: IBclStashMetadata
    abstract member Changes: seq<IBclStashMetadata>

type StashMoveResult(original: StashMoveResponse.Root) =
    member __.Target =
        original.Target.JsonValue.ToString()
        |> (StashMetadataResponse.Parse >> StashMetadata)
    member __.Changes =
        original.Changes
        |> Seq.map (fun m -> m.JsonValue.ToString())
        |> Seq.map (StashMetadataResponse.Parse >> StashMetadata)

    interface IBclStashMoveResult with
        member this.Target = this.Target :> IBclStashMetadata
        member this.Changes = this.Changes |> Seq.map (fun c -> c :> IBclStashMetadata)