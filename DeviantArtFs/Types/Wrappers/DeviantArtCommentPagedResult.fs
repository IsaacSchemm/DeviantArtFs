namespace DeviantArtFs

open System

type IBclDeviantArtCommentPagedResult =
    abstract member HasMore: bool
    abstract member NextOffset: Nullable<int>
    abstract member HasLess: bool
    abstract member PrevOffset: Nullable<int>
    abstract member Total: Nullable<int>
    abstract member Thread: seq<IBclDeviantArtComment>

type DeviantArtCommentPagedResult(original: CommentPagedResponse.Root) =
    member __.HasMore = original.HasMore
    member __.NextOffset = original.NextOffset
    member __.HasLess = original.HasLess
    member __.PrevOffset = original.PrevOffset
    member __.Total = original.Total
    member __.Thread =
        original.Thread
        |> Seq.map (fun c -> c.JsonValue.ToString())
        |> Seq.map (CommentResponse.Parse >> DeviantArtComment)

    interface IBclDeviantArtCommentPagedResult with
        member this.HasMore = this.HasMore
        member this.NextOffset = this.NextOffset |> Option.toNullable
        member this.HasLess = this.HasLess
        member this.PrevOffset = this.PrevOffset |> Option.toNullable
        member this.Total = this.Total |> Option.toNullable
        member this.Thread = this.Thread |> Seq.map (fun c -> c :> IBclDeviantArtComment)
        
    interface IDeviantArtConvertibleToAsyncSeq<DeviantArtComment> with
        member this.HasMore = this.HasMore
        member this.NextOffset = this.NextOffset
        member this.Results = this.Thread