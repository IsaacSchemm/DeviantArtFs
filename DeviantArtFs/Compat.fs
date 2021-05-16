namespace DeviantArtFs

type internal DeviantArtPagingParams = ParameterTypes.Paging

module internal DeviantArtPagingParams =
    let MaxFrom offset = (ParameterTypes.PagingOffset offset, ParameterTypes.MaximumPagingLimit)