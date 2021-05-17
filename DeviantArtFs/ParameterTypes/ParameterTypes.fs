namespace DeviantArtFs.ParameterTypes

open System

type PagingOffset = FromStart | PagingOffset of int
with static member Default = FromStart

type PagingLimit = PagingLimit of int | MaximumPagingLimit | DefaultPagingLimit
with static member Default = DefaultPagingLimit

type ObjectExpansion = StatusFullText | UserDetails | UserGeo | UserProfile | UserStats | UserWatch
with
    static member All = [StatusFullText; UserDetails; UserGeo; UserProfile; UserStats; UserWatch]
    static member None = List.empty<ObjectExpansion>

type ExtParams = ExtSubmission | ExtCamera | ExtStats | ExtCollection
with
    static member All = [ExtSubmission; ExtCamera; ExtStats; ExtCollection]
    static member None = List.empty<ExtParams>

type UserScope = ForCurrentUser | ForUser of string
with static member Default = ForCurrentUser

type CalculateSize = CalculateSize of bool
with static member Default = CalculateSize false

type FolderPreload = FolderPreload of bool
with static member Default = FolderPreload false

type DailyDeviationDate = DailyDeviationsToday | DailyDeviationsFor of DateTime
with static member Default = DailyDeviationsToday

type SearchQuery = NoSearchQuery | SearchQuery of string
with static member Default = NoSearchQuery

type PopularTimeRange = UnspecifiedPopularTimeRange | PopularNow | PopularOneWeek | PopularOneMonth | PopularAllTime
with static member Default = UnspecifiedPopularTimeRange

type UserJournalFilter = NoUserJournalFilter | FeaturedJournalsOnly
with static member Default = FeaturedJournalsOnly

type CommentSubject = OnDeviation of Guid | OnProfile of string | OnStatus of Guid

type CommentReplyType = DirectReply | InReplyToComment of Guid
with static member Default = DirectReply

type CommentDepth = CommentDepth of int
with
    static member Default = CommentDepth 0
    static member Max = CommentDepth 5

type IncludeRelatedItem = IncludeRelatedItem of bool

type EmbeddedDeviationOffset = StartWithFirstEmbeddedDeviation | StartWithEmbeddedDeviation of Guid
with static member Default = StartWithFirstEmbeddedDeviation

type GalleryFolderScope = SingleGalleryFolder of Guid | AllGalleryFoldersNewest | AllGalleryFoldersPopular

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