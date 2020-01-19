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
    member this.GetNextOffset() = OptUtils.toNullable this.next_offset
    member this.GetPrevOffset() = OptUtils.toNullable this.prev_offset