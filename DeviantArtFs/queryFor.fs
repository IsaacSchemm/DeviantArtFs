namespace DeviantArtFs

module internal QueryFor =
    let paging paging (maximum: int) = seq {
        yield sprintf "offset=%d" paging.Offset
        if paging.Limit.HasValue then
            yield sprintf "limit=%d" (min paging.Limit.Value maximum)
    }

    let objectExpansion (p: DeviantArtObjectExpansion) =
        let expand = seq {
            if p.HasFlag(DeviantArtObjectExpansion.UserDetails) then
                yield sprintf "user.details"
            if p.HasFlag(DeviantArtObjectExpansion.UserGeo) then
                yield sprintf "user.geo"
            if p.HasFlag(DeviantArtObjectExpansion.UserProfile) then
                yield sprintf "user.profile"
            if p.HasFlag(DeviantArtObjectExpansion.UserStats) then
                yield sprintf "user.stats"
            if p.HasFlag(DeviantArtObjectExpansion.UserWatch) then
                yield sprintf "user.watch"
        }
        seq {
            if p <> DeviantArtObjectExpansion.None then
                yield expand |> String.concat "," |> sprintf "expand=%s"
        }

    let extParams (extParams: DeviantArtExtParams) = seq {
        yield sprintf "ext_submission=%b" extParams.ExtSubmission
        yield sprintf "ext_camera=%b" extParams.ExtCamera
        yield sprintf "ext_stats=%b" extParams.ExtStats
    }