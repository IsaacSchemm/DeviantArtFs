namespace DeviantArtFs.Stash.Marshal

open DeviantArtFs

type StashRoot() =
    let nodes = new ResizeArray<StashNode>()

    let insert relative_pos (node: StashNode) =
        let siblings =
            nodes
            |> Seq.where (fun n -> n.ParentStackId = node.ParentStackId)
            |> ResizeArray
        if Seq.length siblings <= relative_pos then
            nodes.Add(node)
        else
            let insert_before = siblings.[relative_pos]

            let new_master_pos = nodes.IndexOf(insert_before)
            nodes.Insert(new_master_pos, node)

    let move relative_pos (node: StashNode) =
        let siblings =
            nodes
            |> Seq.where (fun n -> n.ParentStackId = node.ParentStackId)
            |> ResizeArray
        let current_pos = siblings.IndexOf(node)
        let new_pos = current_pos + relative_pos
        let insert_before = siblings.[new_pos]

        let new_master_pos = nodes.IndexOf(insert_before)
        nodes.Remove(node) |> ignore
        nodes.Insert(new_master_pos, node)

    let update pos (node: StashNode) (metadata: StashMetadata) =
        let original_stackid = node.ParentStackId
        node.Metadata <- metadata

        if original_stackid <> node.ParentStackId then
            nodes.Remove(node) |> ignore
            nodes.Insert(0, node)

        match pos with
        | Some s -> node |> move s
        | None -> ()

    member __.Nodes = nodes :> seq<StashNode>
    interface IStashRoot with
        member this.Nodes = this.Nodes

    member __.TryFindItemById itemid =
        seq {
            for node in nodes do
                match node.Metadata.itemid with
                | Some i -> if i = itemid then yield node
                | None -> ()
        } |> Seq.tryHead

    member __.TryFindStackById stackid =
        seq {
            for node in nodes do
                match node.Metadata.itemid with
                | Some _ -> ()
                | None -> if node.Metadata.stackid = Some stackid then yield node
        } |> Seq.tryHead

    member this.FindItemById itemid = Option.get (this.TryFindItemById itemid)
    member this.FindStackById stackid = Option.get (this.TryFindStackById stackid)

    member __.Children = seq {
        for n in nodes do
            printfn "%A" n.ParentStackId
            if Option.isNone n.ParentStackId then
                yield n
    }

    member __.AllItems =
        let rec grab (list: seq<StashNode>) (stackid: int64 option) = seq {
            for n in list do
                if n.ParentStackId = stackid then
                    match n.Metadata.itemid with
                    | Some _ -> yield n
                    | None -> yield! grab n.Children n.Metadata.stackid
        }
        grab nodes None

    member this.Apply (entry: IStashDelta) =
        let itemid = entry.Itemid |> Option.ofNullable
        let stackid = entry.Stackid |> Option.ofNullable
        let metadata = entry.MetadataJson |> Option.ofObj |> Option.map StashMetadata.Parse
        let position = entry.Position |> Option.ofNullable

        match metadata with
        | Some metadata ->
            // Add or update
            match (itemid, stackid) with
            | (Some itemid, _) ->
                // Add or update item
                match this.TryFindItemById itemid with
                | Some existing ->
                    update position existing metadata
                | None ->
                    new StashNode(this, metadata) |> insert (position |> Option.defaultValue 0)
            | (None, Some stackid) ->
                // Add or update stack
                match this.TryFindStackById stackid with
                | Some existing ->
                    update position existing metadata
                | None ->
                    new StashNode(this, metadata) |> insert (position |> Option.defaultValue 0)
            | _ -> failwithf "Invalid combination of stackid/itemid with metadata"
        | None ->
            // Deletion
            match (itemid, stackid) with
            | (Some itemid, None) ->
                // Delete item
                match this.TryFindItemById itemid with
                | Some x -> nodes.Remove(x) |> ignore
                | None -> () // This item's parent stack may have been already removed
            | (None, Some stackid) ->
                // Delete stack
                match this.TryFindStackById stackid with
                | Some x -> nodes.Remove(x) |> ignore
                | None -> () // This stack's parent stack may have been already removed
            | _ -> failwithf "Invalid combination of stackid/itemid without metadata"

    member __.Clear() = nodes.Clear()

    member __.Save() = nodes |> Seq.map (fun n -> n.Save()) |> ResizeArray :> seq<SavedDeltaEntry>