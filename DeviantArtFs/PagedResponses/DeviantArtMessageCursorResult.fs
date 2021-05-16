namespace DeviantArtFs

/// A single page of results from a DeviantArt API message feed.
type DeviantArtMessageCursorResult = {
    cursor: string
    has_more: bool
    results: DeviantArtMessage list
} with
    interface IDeviantArtResultPage<string option, DeviantArtMessage> with
        member this.NextPage =
            match this.has_more with
            | true -> Some (Some this.cursor)
            | false -> None
        member this.Items = this.results |> Seq.ofList