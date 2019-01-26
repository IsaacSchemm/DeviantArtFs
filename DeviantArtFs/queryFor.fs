namespace DeviantArtFs

module internal queryFor =
    let paging (paging: IPagingParams) = seq {
        yield sprintf "offset=%d" paging.Offset
        if paging.Limit.HasValue then
            yield sprintf "limit=%d" paging.Limit.Value
    }

    let extParams (extParams: IExtParams) = seq {
        yield sprintf "ext_submission=%b" extParams.ExtSubmission
        yield sprintf "ext_camera=%b" extParams.ExtCamera
        yield sprintf "ext_stats=%b" extParams.ExtStats
    }

    let fieldChange (name: string) (value: FieldChange<'a>) = seq {
        match value with
        | FieldChange.UpdateToValue s ->
            if obj.ReferenceEquals(s, null) then
                failwithf "Null is not allowed (parameter: %s)" name
            else
                let str = s.ToString()
                yield sprintf "%s=%s" (dafs.urlEncode name) (dafs.urlEncode str)
        | FieldChange.NoChange -> ()
    }