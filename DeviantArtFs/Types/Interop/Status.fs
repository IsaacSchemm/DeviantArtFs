namespace DeviantArtFs.Interop

open DeviantArtFs

[<AllowNullLiteral>]
type Status(original: StatusResponse.Root) =
    member __.Original = original

    member __.StatusId = original.Statusid
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
                | Some s -> yield s.JsonValue.ToString() |> StatusResponse.Parse |> Status
                | None -> ()
    }