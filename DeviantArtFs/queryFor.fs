namespace DeviantArtFs

module internal QueryFor =
    let paging (paging: IDeviantArtPagingParams) (maximum: int) = seq {
        yield sprintf "offset=%d" paging.Offset
        if paging.Limit.HasValue then
            yield sprintf "limit=%d" (max paging.Limit.Value maximum)
    }

    let extParams (extParams: IDeviantArtExtParams) = seq {
        yield sprintf "ext_submission=%b" extParams.ExtSubmission
        yield sprintf "ext_camera=%b" extParams.ExtCamera
        yield sprintf "ext_stats=%b" extParams.ExtStats
    }

    let fieldChange (name: string) (value: DeviantArtFieldChange<'a>) = seq {
        match value with
        | DeviantArtFieldChange.UpdateToValue s ->
            if obj.ReferenceEquals(s, null) then
                failwithf "Null is not allowed (parameter: %s)" name
            else
                let str = s.ToString()
                yield sprintf "%s=%s" (Dafs.urlEncode name) (Dafs.urlEncode str)
        | DeviantArtFieldChange.NoChange -> ()
    }