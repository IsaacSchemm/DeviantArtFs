namespace DeviantArtFs

module internal QueryFor =
    let paging (paging: IDeviantArtPagingParams) (maximum: int) = seq {
        yield sprintf "offset=%d" paging.Offset
        if paging.Limit.HasValue then
            yield sprintf "limit=%d" (min paging.Limit.Value maximum)
    }

    let extParams (extParams: IDeviantArtExtParams) = seq {
        yield sprintf "ext_submission=%b" extParams.ExtSubmission
        yield sprintf "ext_camera=%b" extParams.ExtCamera
        yield sprintf "ext_stats=%b" extParams.ExtStats
    }