namespace DeviantArtFs.Stash.Marshal

open DeviantArtFs.Stash

type StashStack(root: IStashRoot, stackid: int64, metadata: StackResponse.Root) =
    member __.Stackid = stackid
    member val Metadata = metadata with get, set

    member this.Title = this.Metadata.Title |> Option.toObj
    member this.Path = this.Metadata.Path |> Option.toObj
    member this.Size = this.Metadata.Size |> Option.defaultValue 0
    member this.Description = this.Metadata.Description |> Option.toObj
    member this.ParentId = this.Metadata.Parentid
    member this.Thumbnail = this.Metadata.Thumb |> Option.map Utils.toStashFile |> Option.defaultValue null

    member this.Apply (modifications: StackResponse.Root) =
        let v = Utils.apply(this.Metadata, modifications)
        this.Metadata <- v

    interface IStashNode with
        member this.Title = this.Title

    override this.ToString() = this.Title