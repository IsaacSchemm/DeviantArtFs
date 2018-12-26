namespace DeviantArtFs.Stash.Marshal

open DeviantArtFs.Stash

[<AbstractClass>]
type StashNode(root: IStashRoot, metadata: StackResponse.Root) =
    member val Metadata = metadata with get, set
    member this.Title = this.Metadata.Title |> Option.toObj

    member this.Position =
        match root.Nodes |> Seq.tryFindIndex (LanguagePrimitives.PhysicalEquality this) with
        | Some p -> p
        | None -> failwithf "This node is not a member of its root (anymore)"

    abstract member ParentStackId: int64 option
    abstract member Serialize: unit -> DeltaResultEntry

    override this.ToString() = this.Title
and IStashRoot =
    abstract member Nodes: seq<StashNode>