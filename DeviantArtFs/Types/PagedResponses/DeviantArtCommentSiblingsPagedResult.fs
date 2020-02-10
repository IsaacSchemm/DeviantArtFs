namespace DeviantArtFs

open FSharp.Json

type DeviantArtCommentSiblingsContext = {
    parent: DeviantArtComment option
    item_profile: DeviantArtUser option
    item_deviation: Deviation option
    item_status: DeviantArtStatus option
} with
    member this.GetParent() = OptUtils.recordDefault this.parent
    member this.GetItems() = seq {
        yield! OptUtils.toObjSeq this.item_profile
        yield! OptUtils.toObjSeq this.item_deviation
        yield! OptUtils.toObjSeq this.item_status
    }

type DeviantArtCommentSiblingsPagedResult = {
    has_more: bool
    next_offset: int option
    has_less: bool
    prev_offset: int option
    thread: DeviantArtComment list
    context: DeviantArtCommentSiblingsContext
} with
    static member Parse (json: string) =
        json.Replace(""""context": list""", """"context":{}""")
        |> Json.deserialize<DeviantArtCommentSiblingsPagedResult>
    member this.GetNextOffset() = OptUtils.intDefault this.next_offset
    member this.GetPrevOffset() = OptUtils.intDefault this.prev_offset
    interface IResultPage<int, DeviantArtComment> with
        member this.Cursor = this.next_offset |> Option.defaultValue 0
        member this.HasMore = this.has_more
        member this.Items = this.thread |> Seq.ofList