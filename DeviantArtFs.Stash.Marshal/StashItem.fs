namespace DeviantArtFs.Stash.Marshal

open DeviantArtFs.Stash

type StashItem(root: IStashRoot, itemid: int64, metadata: StackResponse.Root) =
    member __.Itemid = itemid
    member val Metadata = metadata with get, set

    member this.Title = this.Metadata.Title |> Option.toObj
    member this.ArtistComments = this.Metadata.ArtistComments |> Option.toObj
    member this.OriginalUrl = this.Metadata.OriginalUrl |> Option.toObj
    member this.Category = this.Metadata.Category |> Option.toObj
    member this.CreationTime = this.Metadata.CreationTime |> Option.toNullable
    member this.Files = this.Metadata.Files |> Seq.map Utils.toStashFile
    member this.Tags = this.Metadata.Tags

    member this.Submission = this.Metadata.Submission
    member this.Stats = this.Metadata.Stats
    member this.Camera = this.Metadata.Camera

    member this.OriginalImageUrl =
        this.Metadata.Files
        |> Seq.sortByDescending (fun f -> f.Width * f.Height)
        |> Seq.map (fun f -> f.Src)
        |> Seq.tryHead
        |> Option.toObj

    member this.OriginalLiteratureUrl =
        this.Metadata.Files
        |> Seq.where (fun f -> f.Width * f.Height = 0)
        |> Seq.map (fun f -> f.Src)
        |> Seq.tryHead
        |> Option.toObj

    member internal this.Apply (modifications: StackResponse.Root) =
        let v = Utils.apply(this.Metadata, modifications)
        this.Metadata <- v

    interface IStashNode with
        member this.Title = this.Title

    override this.ToString() = this.Title