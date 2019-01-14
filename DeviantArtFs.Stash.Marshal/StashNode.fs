namespace DeviantArtFs.Stash.Marshal

open System
open DeviantArtFs

type SavedDeltaEntry =
    {
        Itemid: Nullable<int64>
        Stackid: int64
        MetadataJson: string
        Position: int
    }
    interface ISerializedStashDeltaEntry with
        member this.Itemid = this.Itemid
        member this.Stackid = this.Stackid |> Nullable
        member this.MetadataJson = this.MetadataJson
        member this.Position = this.Position |> Nullable

[<AbstractClass>]
type StashNode(root: IStashRoot, metadata: StashMetadata) =
    member val Metadata = metadata with get, set
    member __.BclMetadata = metadata :> IBclStashMetadata

    member internal this.ParentStackId =
        match this.Metadata.Itemid with
        | Some _ -> this.Metadata.Stackid
        | None -> this.Metadata.Parentid

    member this.Position =
        match root.Nodes |> Seq.tryFindIndex (LanguagePrimitives.PhysicalEquality this) with
        | Some p -> p
        | None -> -1

    abstract member Save: unit -> SavedDeltaEntry

    override __.ToString() = metadata.Title |> Option.defaultValue metadata.Json
and IStashRoot =
    abstract member Nodes: seq<StashNode>

type internal EmptyRoot() =
    interface IStashRoot with
        member __.Nodes = Seq.empty