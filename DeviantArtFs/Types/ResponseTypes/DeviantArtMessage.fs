namespace DeviantArtFs

open System

type DeviantArtMessageSubjectObject = {
    profile: DeviantArtUser option
    deviation: Deviation option
    status: DeviantArtStatus option
    comment: DeviantArtComment option
    collection: DeviantArtCollectionFolder option
    gallery: DeviantArtGalleryFolder option
} with
    member this.Enumerate() = seq {
        match this.profile with | Some s -> s :> obj | None -> ()
        match this.deviation with | Some s -> s :> obj | None -> ()
        match this.status with | Some s -> s :> obj | None -> ()
        match this.comment with | Some s -> s :> obj | None -> ()
        match this.collection with | Some s -> s :> obj | None -> ()
        match this.gallery with | Some s -> s :> obj | None -> ()
    }

type DeviantArtMessage = {
    messageid: string
    ``type``: string
    orphaned: bool

    ts: DateTimeOffset option
    originator: DeviantArtUser option
    subject: DeviantArtMessageSubjectObject option
    
    stackid: string option
    stack_count: int option

    html: string option
    profile: DeviantArtUser option
    deviation: Deviation option
    status: DeviantArtStatus option
    comment: DeviantArtComment option
    collection: DeviantArtCollectionFolder option
} with
    member this.GetTimestampOrNull() = this.ts |> Option.toNullable
    member this.GetOriginators() = this.originator |> Seq.singleton |> Seq.choose id
    member this.GetSubjects() = this.subject |> Option.map (fun s -> s.Enumerate()) |> Option.defaultValue Seq.empty

    member this.GetStackIdOrNull() = this.stackid |> Option.toObj
    member this.GetStackCountOrNull() = this.stack_count |> Option.toNullable

    member this.GetHtmlOrNull() = this.html |> Option.toObj
    member this.GetProfiles() = this.profile |> Seq.singleton |> Seq.choose id
    member this.GetDeviations() = this.deviation |> Seq.singleton |> Seq.choose id
    member this.GetStatuses() = this.status |> Seq.singleton |> Seq.choose id
    member this.GetComments() = this.comment |> Seq.singleton |> Seq.choose id
    member this.GetCollections() = this.collection |> Seq.singleton |> Seq.choose id