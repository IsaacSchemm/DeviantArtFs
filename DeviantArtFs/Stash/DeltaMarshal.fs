namespace DeviantArtFs.Stash.DeltaMarshal

open DeviantArtFs.Stash
open FSharp.Data.Runtime.BaseTypes
open DeviantArtFs.Deviation

module StashPartialMetadataApplication =
    let Apply (original: StackResponse.Root, modifications: StackResponse.Root) =
        let primaryJson = modifications.JsonValue.ToString().Trim()
        let primaryInner = primaryJson.Substring(1, primaryJson.Length - 2).Trim()
        let secondaryJson = original.JsonValue.ToString().Trim()
        let secondaryInner = secondaryJson.Substring(1, secondaryJson.Length - 2).Trim()
        if secondaryInner = "" then
            original
        else if primaryInner = "" then
            modifications
        else
            let parent_json = sprintf """{%s, %s}""" primaryInner secondaryInner
            StackResponse.Parse parent_json

type StashNode =
    abstract member Title: string

type StashPlaceholderNode() =
    interface StashNode with
        member __.Title = ""

type StashItem(itemid: int64, metadata: StackResponse.Root) =
    member __.Itemid = itemid
    member val Metadata = metadata with get, set

    member this.Title = this.Metadata.Title |> Option.toObj
    member this.ArtistComments = this.Metadata.ArtistComments |> Option.toObj
    member this.OriginalUrl = this.Metadata.OriginalUrl |> Option.toObj
    member this.Category = this.Metadata.Category |> Option.toObj
    member this.CreationTime = this.Metadata.CreationTime |> Option.toNullable
    member this.FileUrls = this.Metadata.Files |> Seq.map (fun f -> f.Src)
    member this.Tags = this.Metadata.Tags

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

    interface StashNode with
        member this.Title = this.Title

    member this.Apply (modifications: StackResponse.Root) =
        let v = StashPartialMetadataApplication.Apply(this.Metadata, modifications)
        this.Metadata <- v

type StashStackContainer =
    abstract member Nodes: ResizeArray<StashNode>
    abstract member Stackid: int64 option

type StashStack(stackid: int64, metadata: StackResponse.Root) =
    let nodes = new ResizeArray<StashNode>()

    member __.Stackid = stackid
    member __.Nodes = nodes
    member val Metadata = metadata with get, set

    member this.Title = this.Metadata.Title |> Option.toObj
    member this.Path = this.Metadata.Path |> Option.toObj
    member this.Size = this.Metadata.Size |> Option.defaultValue 0
    member this.Description = this.Metadata.Description |> Option.toObj
    member this.ThumbnailUrl = this.Metadata.Thumb |> Option.map (fun t -> t.Src) |> Option.toObj

    interface StashNode with
        member this.Title = this.Title

    member this.Items = seq {
        for n in this.Nodes do
            match n with
            | :? StashItem as i -> yield i
            | _ -> ()
    }

    member this.Stacks = seq {
        for n in this.Nodes do
            match n with
            | :? StashStack as s -> yield s
            | _ -> ()
    }

    interface StashStackContainer with
        member __.Nodes = nodes
        member __.Stackid = Some stackid

    member this.FindItemsById itemid = seq {
        for i in this.Items do
            if i.Itemid = itemid then
                yield i
        for s in this.Stacks do
            yield! s.FindItemsById itemid
    }

    member this.FindStacksById stackid = seq {
        if this.Stackid = stackid then
            yield this
        for s in this.Stacks do
            yield! s.FindStacksById stackid
    }

    member this.Apply (modifications: StackResponse.Root) =
        let v = StashPartialMetadataApplication.Apply(this.Metadata, modifications)
        this.Metadata <- v

    override this.ToString() = this.Title

module StashUtils =
    let rec findParentsForStack (id: int64) (root: StashStackContainer) = seq {
        for n in root.Nodes do
            match n with
            | :? StashStack as s ->
                if s.Stackid = id then yield root
                yield! findParentsForStack id s
            | _ -> ()
    }

    let findParentForStack id root = findParentsForStack id root |> Seq.tryHead

    let rec findParentsForItem (id: int64) (root: StashStackContainer) = seq {
        for node in root.Nodes do
            match node with
            | :? StashItem as i -> if i.Itemid = id then yield root
            | :? StashStack as s -> yield! findParentsForItem id s
            | _ -> ()
    }

    let findParentForItem id root = findParentsForItem id root |> Seq.tryHead

    let insert (index: int) (item: StashNode) (list: ResizeArray<StashNode>) =
        while list.Count < index do
            list.Add(new StashPlaceholderNode() :> StashNode)
        list.Insert(index, item)

type StashDeltaApplyException() =
    inherit System.Exception()

type StashRoot() =
    let nodes = new ResizeArray<StashNode>()
    let deferred = new System.Collections.Generic.Queue<DeltaResultEntry>()

    member __.Nodes = nodes

    member this.Stacks = seq {
        for n in this.Nodes do
            match n with
            | :? StashStack as s -> yield s
            | _ -> ()
    }

    interface StashStackContainer with
        member __.Nodes = nodes
        member __.Stackid = None

    member this.FindItemById itemid =
        this.Stacks
        |> Seq.collect (fun s -> s.FindItemsById itemid)
        |> Seq.tryHead

    //member this.CreatePlaceholderStack stackid =
    //    printfn "Creating new placeholder stack %d" stackid
    //    let stack = new StashStack(stackid, sprintf "{ \"title\": \"%d\" }" stackid |> StackResponse.Parse)
    //    this.Nodes.Insert(0, stack)
    //    stack

    member this.FindStackById stackid =
        this.Stacks
        |> Seq.collect (fun s -> s.FindStacksById stackid)
        |> Seq.tryHead

    member this.FindStackByIdOrFail stackid =
        match this.FindStackById stackid with
        | Some x -> x
        | None -> raise (new StashDeltaApplyException())

    //member this.FindOrCreateStackById stackid =
    //    this.FindStackById stackid
    //    |> Option.defaultValue (this.CreatePlaceholderStack stackid)

    member this.Apply (delta: DeltaResultEntry) =
        match delta.Metadata with
        | Some metadata ->
            // Add or update
            match (delta.Itemid, delta.Stackid) with
            | (Some itemid, Some stackid) ->
                // Add or update item
                let new_parent = this.FindStackByIdOrFail stackid
                match this.FindItemById itemid with
                | Some existing ->
                    // Update item
                    existing.Apply metadata

                    let old_parent = (StashUtils.findParentForItem itemid this).Value
                    if old_parent.Stackid <> Some new_parent.Stackid then
                        old_parent.Nodes.Remove(existing) |> ignore
                        match delta.Position with
                        | Some pos -> new_parent.Nodes |> StashUtils.insert pos existing
                        | None -> new_parent.Nodes.Add(existing)
                    else
                        match delta.Position with
                        | Some pos ->
                            let current_pos = old_parent.Nodes.IndexOf(existing)
                            let new_pos = current_pos + pos
                            old_parent.Nodes.Remove(existing) |> ignore
                            old_parent.Nodes |> StashUtils.insert new_pos existing
                        | None -> ()
                    ()
                | None ->
                    // Add item
                    let new_item = new StashItem(itemid, metadata)
                    match delta.Position with
                    | Some pos -> new_parent.Nodes |> StashUtils.insert pos new_item
                    | None -> new_parent.Nodes.Add(new_item)
            | (None, Some stackid) ->
                // Add or update stack
                let new_parent =
                    match metadata.Parentid with
                    | Some parentid -> this.FindStackByIdOrFail parentid :> StashStackContainer
                    | None -> this :> StashStackContainer
                match this.FindStackById stackid with
                | Some existing ->
                    // Update item
                    existing.Apply metadata

                    let old_parent = (StashUtils.findParentForStack stackid this).Value
                    if old_parent.Stackid <> new_parent.Stackid then
                        old_parent.Nodes.Remove(existing) |> ignore
                        printfn "%O %O" new_parent.Stackid delta.Position
                        match delta.Position with
                        | Some pos -> new_parent.Nodes |> StashUtils.insert pos existing
                        | None -> new_parent.Nodes.Add(existing)
                    else
                        match delta.Position with
                        | Some pos ->
                            let current_pos = old_parent.Nodes.IndexOf(existing)
                            let new_pos = current_pos + pos
                            old_parent.Nodes.Remove(existing) |> ignore
                            old_parent.Nodes |> StashUtils.insert new_pos existing
                        | None -> ()
                    ()
                | None ->
                    // Add stack
                    let new_stack = new StashStack(stackid, metadata)
                    match delta.Position with
                    | Some pos -> new_parent.Nodes |> StashUtils.insert pos new_stack
                    | None -> new_parent.Nodes.Add(new_stack)
            | _ -> failwithf "Invalid combination of stackid/itemid with metadata"
        | None ->
            // Deletion
            match (delta.Itemid, delta.Stackid) with
            | (Some itemid, None) ->
                // Delete item
                let parent = (StashUtils.findParentForItem itemid this).Value
                let item = (this.FindItemById itemid).Value
                parent.Nodes.Remove(item) |> ignore
            | (None, Some stackid) ->
                // Delete stack
                let parent = (StashUtils.findParentForStack stackid this).Value
                let stack = (this.FindStackById stackid).Value
                parent.Nodes.Remove(stack) |> ignore
            | _ -> failwithf "Invalid combination of stackid/itemid without metadata"

    member __.DeferredCount = deferred.Count

    member this.ApplyDeferred() =
        let initialSize = deferred.Count
        for _ in [1..initialSize] do
            let first = deferred.Dequeue()
            try
                this.Apply first
            with
                | :? StashDeltaApplyException -> deferred.Enqueue(first)
        let processed = initialSize - deferred.Count
        if processed > 0 then
            this.ApplyDeferred()

    member this.Defer delta =
        printfn "Deferring: %O %O" delta.Stackid delta.Itemid
        deferred.Enqueue(delta)

    member this.ApplyOrDefer delta =
        try
            this.Apply delta
            true
        with
            | :? StashDeltaApplyException ->
                this.Defer delta
                false