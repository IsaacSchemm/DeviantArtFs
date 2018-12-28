namespace DeviantArtFs.Stash.Marshal

open System
open DeviantArtFs.Stash
open DeviantArtFs

type SavedDeltaEntry =
    {
        Itemid: Nullable<int64>
        Stackid: int64
        Metadata: string
        Position: int
    }
    interface IDeltaEntry with
        member this.Itemid = this.Itemid
        member this.Stackid = this.Stackid |> Nullable
        member this.Metadata = this.Metadata
        member this.Position = this.Position |> Nullable

[<AbstractClass>]
type StashNode(root: IStashRoot, metadata: StackOrItemResponse.Root) =
    member val Metadata = metadata with get, set
    member this.Title = this.Metadata.Title |> Option.toObj

    member this.Position =
        match root.Nodes |> Seq.tryFindIndex (LanguagePrimitives.PhysicalEquality this) with
        | Some p -> p
        | None -> failwithf "This node is not a member of its root (anymore)"

    abstract member ParentStackId: int64 option
    abstract member Save: unit -> SavedDeltaEntry

    override this.ToString() = this.Title
and IStashRoot =
    abstract member Nodes: seq<StashNode>