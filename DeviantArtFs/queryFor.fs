namespace DeviantArtFs

module internal queryFor =
    let paging (paging: IDeviantArtPagingParams) = seq {
        yield sprintf "offset=%d" paging.Offset
        if paging.Limit.HasValue then
            yield sprintf "limit=%d" paging.Limit.Value
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
                yield sprintf "%s=%s" (dafs.urlEncode name) (dafs.urlEncode str)
        | DeviantArtFieldChange.NoChange -> ()
    }