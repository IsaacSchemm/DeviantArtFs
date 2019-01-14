namespace DeviantArtFs.Stash.Marshal

open DeviantArtFs

type StashItem(root: IStashRoot, itemid: int64, metadata: StashMetadata) =
    inherit StashNode(root, metadata)

    member __.Itemid = itemid

    override this.Save() = {
        Itemid = this.Itemid |> System.Nullable
        Stackid = this.ParentStackId.Value
        MetadataJson = this.Metadata.Json
        Position = this.Position
    }