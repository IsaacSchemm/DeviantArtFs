namespace DeviantArtFs.Stash.Marshal

open DeviantArtFs.Stash

type StashRoot() =
    let nodes = new ResizeArray<StashNode>()

    interface IStashRoot with
        member __.Nodes = nodes :> seq<StashNode>

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
        let rec grab (list: seq<StashNode>) = seq {
            for n in list do
                match n with
                | :? StashItem as i -> yield i
                | :? StashStack as s -> yield! grab s.Children
                | _ -> ()
        }
        grab nodes

    member this.Apply (delta: DeltaResultEntry) =
        match delta.Metadata with
        | Some metadata ->
            // Add or update
            match (delta.Itemid, delta.Stackid) with
            | (Some itemid, Some stackid) ->
                // Add or update item
                match this.FindItemById itemid with
                | Some existing ->
                    // Update item
                    existing.Apply metadata

                    match delta.Position with
                    | Some pos ->
                        let siblings =
                            nodes
                            |> Seq.where (fun n -> n.ParentStackId = existing.ParentStackId)
                            |> ResizeArray
                        let current_pos = siblings.IndexOf(existing)
                        let new_pos = current_pos + pos
                        let insert_before = siblings.[new_pos]

                        let new_master_pos = nodes.IndexOf(insert_before)
                        nodes.Remove(existing) |> ignore
                        nodes.Insert(System.Math.Min(new_master_pos, nodes.Count), existing)
                    | None -> ()
                | None ->
                    // Add item
                    let new_item = new StashItem(this, itemid, metadata)
                    match delta.Position with
                    | Some pos -> nodes.Insert(System.Math.Min(pos, nodes.Count), new_item)
                    | None -> nodes.Add(new_item)
            | (None, Some stackid) ->
                // Add or update stack
                match this.FindStackById stackid with
                | Some existing ->
                    // Update item
                    existing.Apply metadata

                    match delta.Position with
                    | Some pos ->
                        let siblings =
                            nodes
                            |> Seq.where (fun n -> n.ParentStackId = existing.ParentStackId)
                            |> ResizeArray
                        let current_pos = siblings.IndexOf(existing)
                        let new_pos = current_pos + pos
                        let insert_before = siblings.[new_pos]

                        let new_master_pos = nodes.IndexOf(insert_before)
                        nodes.Remove(existing) |> ignore
                        nodes.Insert(System.Math.Min(new_master_pos, nodes.Count), existing)
                    | None -> ()
                | None ->
                    // Add stack
                    let new_stack = new StashStack(this, stackid, metadata)
                    match delta.Position with
                    | Some pos -> nodes.Insert(System.Math.Min(pos, nodes.Count), new_stack)
                    | None -> nodes.Add(new_stack)
            | _ -> failwithf "Invalid combination of stackid/itemid with metadata"
        | None ->
            // Deletion
            match (delta.Itemid, delta.Stackid) with
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