namespace DeviantArtFs

open DeviantArtFs.ParameterTypes

module internal QueryFor =
    let offset offset = seq {
        match offset with
        | FromStart -> ()
        | PagingOffset o -> sprintf "offset=%d" o
    }

    let limit limit maximum = seq {
        match limit with
        | PagingLimit l -> sprintf "limit=%d" (min l maximum)
        | MaximumPagingLimit -> sprintf "limit=%d" maximum
        | DefaultPagingLimit -> ()
    }

    let paging (offset_p, limit_p) maximum = seq {
        yield! offset offset_p
        yield! limit limit_p maximum
    }

    let objectExpansion objectExpansion = seq {
        let to_include = List.distinct [
            for x in Seq.distinct objectExpansion do
                match x with
                | StatusFullText -> "status.fulltext"
                | UserDetails -> "user.details"
                | UserGeo -> "user.geo"
                | UserProfile -> "user.profile"
                | UserStats -> "user.stats"
                | UserWatch -> "user.watch"
        ]
        match to_include with
        | [] -> ()
        | list -> sprintf "expand=%s" (String.concat "," list)
    }

    let extParams extParams = seq {
        for x in Seq.distinct extParams do
            match x with
            | ExtSubmission -> "ext_submission=1"
            | ExtCamera -> "ext_camera=1"
            | ExtStats -> "ext_stats=1"
    }