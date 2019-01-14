namespace DeviantArtFs.Stash.Marshal

open DeviantArtFs
open DeviantArtFs.Requests.Stash

type StashStack internal (root: IStashRoot, stackid: int64, metadata: StashMetadata) =
    inherit StashNode(root, metadata)

    member __.Stackid = stackid

    member this.Path = this.Metadata.Path |> Option.toObj
    member this.Size = this.Metadata.Size |> Option.defaultValue 0
    member this.Description = this.Metadata.Description |> Option.toObj
    member this.Thumbnail = this.Metadata.Thumb |> Option.toObj

    override this.ParentStackId = this.Metadata.Parentid

    override this.Save() = {
        Itemid = System.Nullable()
        Stackid = this.Stackid
        MetadataJson = this.Metadata.Json
        Position = this.Position
    }

    member __.Children = seq {
        for n in root.Nodes do
            if n.ParentStackId = Some stackid then
                yield n
    }

    member this.Stacks = seq {
        for n in this.Children do
            match n with
            | :? StashStack as s -> yield s
            | _ -> ()
    }

    member this.Items = seq {
        for n in this.Children do
            match n with
            | :? StashItem as i -> yield i
            | _ -> ()
    }