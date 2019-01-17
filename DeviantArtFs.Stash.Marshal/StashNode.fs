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
    interface IStashDelta with
        member this.Itemid = this.Itemid
        member this.Stackid = this.Stackid |> Nullable
        member this.MetadataJson = this.MetadataJson
        member this.Position = this.Position |> Nullable

type StashNode(root: IStashRoot, metadata: StashMetadata) =
    member val Metadata = metadata with get, set

    member this.BclMetadata = this.Metadata :> IBclStashMetadata

    member internal this.ParentStackId =
        match this.Metadata.itemid with
        | Some _ -> this.Metadata.stackid
        | None -> this.Metadata.parentid

    member this.Position =
        match root.Nodes |> Seq.tryFindIndex (LanguagePrimitives.PhysicalEquality this) with
        | Some p -> p
        | None -> -1

    member this.Save() = {
        Itemid = this.Metadata.itemid |> Option.toNullable
        Stackid = this.Metadata.stackid |> Option.get
        MetadataJson = this.Metadata.Json
        Position = this.Position
    }

    member this.Children =
        match this.Metadata.itemid with
        | Some _ -> Seq.empty
        | None -> seq {
            for n in root.Nodes do
                if n.ParentStackId = this.Metadata.stackid then
                    yield n
        }

    member this.Stacks =
        match this.Metadata.itemid with
        | Some _ -> Seq.empty
        | None -> seq {
            for n in this.Children do
                match n.Metadata.itemid with
                | Some _ -> ()
                | None -> yield n
        }

    member this.Items =
        match this.Metadata.itemid with
        | Some _ -> Seq.empty
        | None -> seq {
            for n in this.Children do
                match n.Metadata.itemid with
                | Some _ -> yield n
                | None -> ()
        }

    override this.ToString() = this.Metadata.title
and IStashRoot =
    abstract member Nodes: seq<StashNode>
