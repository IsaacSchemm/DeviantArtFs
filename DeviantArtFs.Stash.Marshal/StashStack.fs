namespace DeviantArtFs.Stash.Marshal

open DeviantArtFs
open DeviantArtFs.Requests.Stash

type StashStack internal (root: IStashRoot, stackid: int64, metadata: StashMetadata.Root) =
    inherit StashNode(root, metadata)

    static member AsyncGetStack token stackid = async {
        let! resp = Stack.AsyncExecute token stackid
        return new StashStack(new EmptyRoot(), stackid, resp)
    }

    static member AsyncGetContents token stackid = async {
        let root = new EmptyRoot()
        let! resp =
            match stackid with
            | Some s -> Contents.AsyncExecute token s
            | None -> Contents.AsyncGetRoot token
        return {
            HasMore = resp.HasMore
            NextOffset = resp.NextOffset
            Results = seq {
                for r in resp.Results do
                    match (r.Itemid, r.Stackid) with
                    | (Some itemid, _) -> yield new StashItem(root, itemid, r) :> StashNode
                    | (None, Some stackid) -> yield new StashStack(root, stackid, r) :> StashNode
                    | _ -> ()
            }
        }
    }

    static member GetStackAsync token stackid =
        stackid
        |> StashStack.AsyncGetStack token
        |> Async.StartAsTask
    static member GetContentsAsync token stackid =
        stackid
        |> Option.ofNullable
        |> StashStack.AsyncGetContents token
        |> Async.StartAsTask

    member __.Stackid = stackid

    member this.Path = this.Metadata.Path |> Option.toObj
    member this.Size = this.Metadata.Size |> Option.defaultValue 0
    member this.Description = this.Metadata.Description |> Option.toObj
    member this.Thumbnail = this.Metadata.Thumb |> Option.map Utils.toStashFile |> Option.defaultValue null

    override this.ParentStackId = this.Metadata.Parentid

    override this.Save() = {
        Itemid = System.Nullable()
        Stackid = this.Stackid
        Metadata = this.Metadata.JsonValue.ToString()
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