namespace DeviantArtFs

open System

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

    // TODO - Reusing this interface seems confusing
    interface IBclDeviantArtPagedResult<DeviantArtComment> with
        member this.HasMore = this.HasMore
        member this.NextOffset = this.NextOffset |> Option.toNullable
        member this.HasLess = Nullable(this.HasLess)
        member this.PrevOffset = this.PrevOffset |> Option.toNullable
        member this.EstimatedTotal = this.Total |> Option.toNullable
        member this.Name = null
        member this.Results = this.Thread