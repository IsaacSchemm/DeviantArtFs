namespace DeviantArtFs

type DeviationTag = {
    tag_name: string
    sponsored: bool
    sponsor: string option
} with
    member this.GetSponsor() = OptUtils.stringDefault this.sponsor