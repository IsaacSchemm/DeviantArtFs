namespace DeviantArtFs

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
        Userid = original.Author.Userid
        Username = original.Author.Username
        Usericon = original.Author.Usericon
        Type = original.Author.Type
    }

    member __.EmbeddedDeviations = seq {
        for i in original.Items do
            match i.Deviation with
                | Some s -> yield s.JsonValue.ToString() |> DeviationResponse.Parse |> Deviation
                | None -> ()
    }

    member __.EmbeddedStatuses = seq {
        for i in original.Items do
            match i.Status with
                | Some s -> yield s.JsonValue.ToString() |> StatusResponse.Parse |> Status
                | None -> ()
    }