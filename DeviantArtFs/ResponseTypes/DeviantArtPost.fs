namespace DeviantArtFs

type DeviantArtPost = {
    journal: Deviation option
    status: DeviantArtStatus option
} with
    member this.GetJournalObjects() =
        match this.journal with
        | Some j -> Seq.singleton j
        | None -> Seq.empty
    member this.GetStatusObjects() =
        match this.status with
        | Some s -> Seq.singleton s
        | None -> Seq.empty