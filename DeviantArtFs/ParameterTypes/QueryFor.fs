namespace DeviantArtFs.ParameterTypes

module QueryFor =
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
            | ExtCollection -> "ext_collection=1"
    }

    let userScope scope = seq {
        match scope with
        | ForUser s -> yield sprintf "username=%s" (System.Uri.EscapeDataString s)
        | ForCurrentUser -> ()
    }

    let calculateSize calculateSize = seq {
        match calculateSize with
        | CalculateSize true -> yield "calculate_size=1"
        | CalculateSize false -> yield "calculate_size=0"
    }

    let folderPreload extPreload = seq {
        match extPreload with
        | FolderPreload true -> yield "ext_preload=1"
        | FolderPreload false -> yield "ext_preload=0"
    }

    let dailyDeviationDate date = seq {
        match date with
        | DailyDeviationsFor d -> yield d.ToString("YYYY-MM-dd") |> sprintf "date=%s"
        | DailyDeviationsToday -> ()
    }

    let searchQuery q = seq {
        match q with
        | SearchQuery s -> yield sprintf "q=%s" (System.Uri.EscapeDataString s)
        | NoSearchQuery -> ()
    }

    let timeRange timerange = seq {
        match timerange with
        | PopularNow -> yield "timerange=now"
        | PopularOneWeek -> yield "timerange=1week"
        | PopularOneMonth -> yield "timerange=1month"
        | PopularAllTime -> yield "timerange=alltime"
        | UnspecifiedPopularTimeRange -> ()
    }

    let userJournalFilter filter = seq {
        match filter with
        | NoUserJournalFilter -> yield "featured=0"
        | FeaturedJournalsOnly -> yield "featured=1"
    }

    let commentReplyType commentReplyType = seq {
        match commentReplyType with
        | DirectReply -> ()
        | InReplyToComment g -> sprintf "commentid=%O" g
    }

    let commentDepth depth = seq {
        match depth with
        | CommentDepth x -> sprintf "maxdepth=%d" (min x 5)
    }

    let includeRelatedItem inc = seq {
        match inc with
        | IncludeRelatedItem true -> "ext_item=1"
        | IncludeRelatedItem false -> "ext_item=0"
    }

    let embeddedDeviationOffset embeddedDeviationOffset = seq {
        match embeddedDeviationOffset with
        | StartWithFirstEmbeddedDeviation -> ()
        | StartWithEmbeddedDeviation g -> sprintf "offset_deviationid=%O" g
    }