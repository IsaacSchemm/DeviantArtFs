namespace DeviantArtFs

open System

[<AllowNullLiteral>]
type IBclDeviantArtStatus =
    abstract member Statusid: Guid
    abstract member Body: string
    abstract member Ts: DateTimeOffset
    abstract member Url: string
    abstract member CommentsCount: int
    abstract member IsShare: bool
    abstract member IsDeleted: bool
    abstract member Author: IDeviantArtUser
    abstract member EmbeddedDeviations: seq<IBclDeviation>
    abstract member EmbeddedStatuses: seq<IBclDeviantArtStatus>

type DeviantArtStatus(original: StatusResponse.Root) =
    member __.Statusid = original.Statusid
    member __.Body = original.Body
    member __.Ts = original.Ts
    member __.Url = original.Url
    member __.CommentsCount = original.CommentsCount
    member __.IsShare = original.IsShare
    member __.IsDeleted = original.IsDeleted
    member __.Author = {
        new IDeviantArtUser with
            member __.Userid = original.Author.Userid
            member __.Username = original.Author.Username
            member __.Usericon = original.Author.Usericon
            member __.Type = original.Author.Type
    }

    member __.EmbeddedDeviations = seq {
        for i in original.Items do
            match i.Deviation with
                | Some s -> yield s.JsonValue.ToString() |> DeviationResponse.Parse |> Deviation :> IBclDeviation
                | None -> ()
    }

    member __.EmbeddedStatuses = seq {
        for i in original.Items do
            match i.Status with
                | Some s -> yield s.JsonValue.ToString() |> StatusResponse.Parse |> DeviantArtStatus
                | None -> ()
    }

    interface IBclDeviantArtStatus with
        member this.Body = this.Body
        member this.CommentsCount = this.CommentsCount
        member this.EmbeddedDeviations = this.EmbeddedDeviations
        member this.EmbeddedStatuses = this.EmbeddedStatuses |> Seq.map (fun s -> s :> IBclDeviantArtStatus)
        member this.IsDeleted = this.IsDeleted
        member this.IsShare = this.IsShare
        member this.Statusid = this.Statusid
        member this.Ts = this.Ts
        member this.Url = this.Url
        member this.Author = this.Author