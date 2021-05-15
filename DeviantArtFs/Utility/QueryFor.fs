namespace DeviantArtFs

open DeviantArtFs.ParameterTypes

module internal QueryFor =
    let paging paging maximum = seq {
        yield sprintf "offset=%d" paging.Offset
        if paging.Limit.HasValue then
            yield sprintf "limit=%d" (min paging.Limit.Value maximum)
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