namespace DeviantArtFs

module internal QueryFor =
    let paging paging (maximum: int) = seq {
        yield sprintf "offset=%d" paging.Offset
        if paging.Limit.HasValue then
            yield sprintf "limit=%d" (min paging.Limit.Value maximum)
    }

    let commonParams (p: DeviantArtCommonParams) =
        let expand = seq {
            if p.Expand.HasFlag(DeviantArtObjectExpansion.UserDetails) then
                yield sprintf "user.details"
            if p.Expand.HasFlag(DeviantArtObjectExpansion.UserGeo) then
                yield sprintf "user.geo"
            if p.Expand.HasFlag(DeviantArtObjectExpansion.UserProfile) then
                yield sprintf "user.profile"
            if p.Expand.HasFlag(DeviantArtObjectExpansion.UserStats) then
                yield sprintf "user.stats"
            if p.Expand.HasFlag(DeviantArtObjectExpansion.UserWatch) then
                yield sprintf "user.watch"
        }
        seq {
            if p.MatureContent then
                yield sprintf "mature_content=true"
            if p.Expand <> DeviantArtObjectExpansion.None then
                yield expand |> String.concat "," |> sprintf "expand=%s"
        }

    let extParams (extParams: DeviantArtExtParams) = seq {
        yield sprintf "ext_submission=%b" extParams.ExtSubmission
        yield sprintf "ext_camera=%b" extParams.ExtCamera
        yield sprintf "ext_stats=%b" extParams.ExtStats
    }