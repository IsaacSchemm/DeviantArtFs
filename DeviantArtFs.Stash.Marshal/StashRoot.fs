﻿namespace DeviantArtFs.Stash.Marshal

open DeviantArtFs

type StashRoot() =
    let nodes = new ResizeArray<StashNode>()

    let insert absolute_pos (node: StashNode) =
        nodes.Remove(node) |> ignore
        nodes.Insert(absolute_pos, node)

    let move relative_pos (node: StashNode) =
        let siblings =
            nodes
            |> Seq.where (fun n -> n.ParentStackId = node.ParentStackId)
            |> ResizeArray
        let current_pos = siblings.IndexOf(node)
        let new_pos = current_pos + relative_pos
        let insert_before = siblings.[new_pos]

        let new_master_pos = nodes.IndexOf(insert_before)
        insert new_master_pos node

    let update pos (node: StashNode) (metadata: StashMetadata) =
        let original_stackid = node.ParentStackId
        node.Metadata <- metadata

        if original_stackid <> node.ParentStackId then
            node |> insert 0

        match pos with
        | Some s -> node |> move s
        | None -> ()

    member __.Nodes = nodes :> seq<StashNode>
    interface IStashRoot with
        member this.Nodes = this.Nodes

    member __.FindItemById itemid =
        seq {
            for node in nodes do
                match node with
                | :? StashItem as i -> if i.Itemid = itemid then yield i
                | _ -> ()
        } |> Seq.tryHead

    member __.FindStackById stackid =
        seq {
            for node in nodes do
                match node with
                | :? StashStack as s -> if s.Stackid = stackid then yield s
                | _ -> ()
        } |> Seq.tryHead

    member __.Children = seq {
        for n in nodes do
            if Option.isNone n.ParentStackId then
                yield n
    }

    member __.AllItems =
        let rec grab (list: seq<StashNode>) (stackid: int64 option) = seq {
            for n in list do
                if n.ParentStackId = stackid then
                    match n with
                    | :? StashItem as i -> yield i
                    | :? StashStack as s -> yield! grab s.Children (Some s.Stackid)
                    | _ -> ()
        }
        grab nodes None

    member this.Apply (entry: ISerializedStashDeltaEntry) =
        let itemid = entry.Itemid |> Option.ofNullable
        let stackid = entry.Stackid |> Option.ofNullable
        let metadata = entry.MetadataJson |> Option.ofObj |> Option.map (StashMetadataResponse.Parse >> StashMetadata)
        let position = entry.Position |> Option.ofNullable

        match metadata with
        | Some metadata ->
            // Add or update
            match (itemid, stackid) with
            | (Some itemid, _) ->
                // Add or update item
                match this.FindItemById itemid with
                | Some existing ->
                    update position existing metadata
                | None ->
                    new StashItem(this, itemid, metadata) |> insert (position |> Option.defaultValue 0)
            | (None, Some stackid) ->
                // Add or update stack
                match this.FindStackById stackid with
                | Some existing ->
                    update position existing metadata
                | None ->
                    new StashStack(this, stackid, metadata) |> insert (position |> Option.defaultValue 0)
            | _ -> failwithf "Invalid combination of stackid/itemid with metadata"
        | None ->
            // Deletion
            match (itemid, stackid) with
            | (Some itemid, None) ->
                // Delete item
                match this.FindItemById itemid with
                | Some x -> nodes.Remove(x) |> ignore
                | None -> () // This item's parent stack may have been already removed
            | (None, Some stackid) ->
                // Delete stack
                match this.FindStackById stackid with
                | Some x -> nodes.Remove(x) |> ignore
                | None -> () // This stack's parent stack may have been already removed
            | _ -> failwithf "Invalid combination of stackid/itemid without metadata"

    member __.Clear() = nodes.Clear()

    member __.Save() = nodes |> Seq.map (fun n -> n.Save()) |> ResizeArray :> seq<SavedDeltaEntry>