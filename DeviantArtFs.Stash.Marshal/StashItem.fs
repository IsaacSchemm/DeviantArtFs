namespace DeviantArtFs.Stash.Marshal

open DeviantArtFs.Stash

type StashItem(root: IStashRoot, itemid: int64, metadata: StackResponse.Root) =
    inherit StashNode(root, metadata)

    member __.Itemid = itemid

    member this.ArtistComments = this.Metadata.ArtistComments |> Option.toObj
    member this.OriginalUrl = this.Metadata.OriginalUrl |> Option.toObj
    member this.Category = this.Metadata.Category |> Option.toObj
    member this.CreationTime = this.Metadata.CreationTime |> Option.toNullable
    member this.Files = this.Metadata.Files |> Seq.map Utils.toStashFile
    member this.Tags = this.Metadata.Tags

    member this.OptSubmission = this.Metadata.Submission
    member this.OptStats = this.Metadata.Stats
    member this.OptCamera = this.Metadata.Camera
    
    override this.ParentStackId =
        match this.Metadata.Stackid with
        | Some s -> Some s
        | None -> failwithf "Item %d does not belong to a stack" itemid

    override this.Serialize() = {
        Itemid = Some this.Itemid
        Stackid = this.ParentStackId
        Metadata = Some this.Metadata
        Position = Some this.Position
    }

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