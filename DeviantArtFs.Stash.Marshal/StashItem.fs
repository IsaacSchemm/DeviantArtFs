namespace DeviantArtFs.Stash.Marshal

open DeviantArtFs
open DeviantArtFs.Requests.Stash

type StashItem(root: IStashRoot, itemid: int64, metadata: StashMetadata) =
    inherit StashNode(root, metadata)

    static member AsyncGetItem token req = async {
        let! resp = Item.AsyncExecute token req
        return new StashItem(new EmptyRoot(), req.Itemid, resp)
    }

    static member GetItemAsync token itemid = StashItem.AsyncGetItem token itemid |> Async.StartAsTask

    member __.Itemid = itemid

    member this.ArtistComments = this.Metadata.ArtistComments |> Option.toObj
    member this.Tags = this.Metadata.Tags
    member this.OriginalUrl = this.Metadata.OriginalUrl |> Option.toObj
    member this.Category = this.Metadata.Category |> Option.toObj
    member this.CreationTime = this.Metadata.CreationTime |> Option.toNullable
    member this.Files = this.Metadata.Files

    member this.Submission = this.Metadata.Submission |> Option.map (fun s -> s :> IBclStashSubmission) |> Option.toObj
    member this.Stats = this.Metadata.Stats |> Option.map (fun s -> s :> IBclStashStats) |> Option.toObj
    member this.CameraJson = this.Metadata.CameraJson |> Option.toObj
    
    override this.ParentStackId =
        match this.Metadata.Stackid with
        | Some s -> Some s
        | None -> failwithf "Item %d does not belong to a stack" itemid

    override this.Save() = {
        Itemid = this.Itemid |> System.Nullable
        Stackid = this.ParentStackId.Value
        MetadataJson = this.Metadata.Json
        Position = this.Position
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