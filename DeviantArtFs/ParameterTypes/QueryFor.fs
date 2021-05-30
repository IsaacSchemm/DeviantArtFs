namespace DeviantArtFs.ParameterTypes

open System

module QueryFor =
    let offset offset = seq {
        match offset with
        | StartingOffset -> ()
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
        | PopularTimeRangeUnspecified -> ()
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

    let messageFolder folder = seq {
        match folder with
        | MessageFolder g -> sprintf "folderid=%O" g
        | Inbox -> ()
    }

    let stackMessages stack = seq {
        match stack with
        | StackMessages true -> "stack=1"
        | StackMessages false -> "stack=0"
    }

    let messageCursor cursor = seq {
        match cursor with
        | MessageCursor s -> sprintf "cursor=%s" (Uri.EscapeDataString s)
        | StartingCursor -> ()
    }

    let messageDeletionTarget target = seq {
        match target with
        | DeleteMessage s -> sprintf "messageid=%s" (Uri.EscapeDataString s)
        | DeleteStack s -> sprintf "stackid=%s" (Uri.EscapeDataString s)
    }

    let feedbackMessageType t = seq {
        match t with
        | CommentFeedbackMessages -> "type=comments"
        | ReplyFeedbackMessages -> "type=replies"
        | ActivityFeedbackMessages -> "type=activity"
    }

    let stashItem item = seq {
        match item with
        | StashItem id -> sprintf "itemid=%d" id
    }

    let stashDeltaCursor cursor = seq {
        match cursor with
        | StashDeltaCursor c -> sprintf "cursor=%s" (Uri.EscapeDataString c)
        | InitialStashDeltaRequest -> ()
    }

    let publishParameters (publishParameters: PublishParameters) = seq {
        match publishParameters.maturity with
        | NotMature ->
            "is_mature=0"
        | Mature (level, MatureClassificationSet classifications) ->
            "is_mature=1"
            match level with
            | MatureStrict -> "mature_level=strict"
            | MatureModerate -> "mature_level=moderate"
            for classification in classifications do
                match classification with
                | Nudity -> "mature_classification[]=nudity"
                | Sexual -> "mature_classification[]=sexual"
                | Gore -> "mature_classification[]=gore"
                | Language -> "mature_classification[]=language"
                | Ideology -> "mature_classification[]=ideology"

        match publishParameters.submissionPolicyAgreement with
        | true -> "agree_submission=1"
        | false -> "agree_submission=0"
        match publishParameters.termsOfServiceAgreement with
        | true -> "agree_tos=1"
        | false -> "agree_tos=0"

        match publishParameters.featured with
        | true -> "feature=1"
        | false -> "feature=0"
        match publishParameters.allowComments with
        | true -> "allow_comments=1"
        | false -> "allow_comments=0"
        match publishParameters.requestCritique with
        | true -> "request_critique=1"
        | false -> "request_critique=0"

        sprintf "display_resolution=%d" (int publishParameters.displayResolution)

        match publishParameters.sharing with
        | AllowSharing -> "sharing=allow"
        | HideShareButtons -> "sharing=hide_share_buttons"
        | HideShareButtonsAndMembersOnly -> "sharing=hide_and_members_only"

        match publishParameters.license with
        | DefaultLicense ->
            "license_options[creative_commons]=0"
        | CreativeCommonsLicense license ->
            "license_options[creative_commons]=1"
            match license.commercialUse with
            | CommercialUsePermitted -> "license_options[commercial]=yes"
            | NonCommercial -> "license_options[commercial]=no"
            match license.derivativeWorks with
            | DerivativeWorksPermitted -> "license_options[modify]=yes"
            | NoDerivatives -> "license_options[modify]=no"
            | ShareAlike -> "license_options[modify]=share"

        match publishParameters.destinations with
        | GallerySet set ->
            for g in set do
                sprintf "galleryids[]=%O" g

        match publishParameters.allowFreeDownload with
        | true -> "allow_free_download=1"
        | false -> "allow_free_download=0"
        match publishParameters.addWatermark with
        | true -> "add_watermark=1"
        | false -> "add_watermark=0"
    }

    let stackModification stackModification = seq {
        match stackModification with
        | ModifyStackTitle v -> yield sprintf "title=%s" (Uri.EscapeDataString v)
        | ModifyStackDescription v -> yield sprintf "description=%s" (Uri.EscapeDataString v)
        | ClearStackDescription -> yield "description=null"
    }

    let additionalUser additionalUser = seq {
        match additionalUser with
        | AdditionalUser u -> yield sprintf "search=%s" (Uri.EscapeDataString u)
        | NoAdditionalUser -> ()
    }

    let watchTypes watchTypes = seq {
        let w = Seq.cache watchTypes
        yield sprintf "watch[friend]=%b" (Seq.contains MakeFriend w)
        yield sprintf "watch[deviations]=%b" (Seq.contains WatchDeviations w)
        yield sprintf "watch[journals]=%b" (Seq.contains WatchJournals w)
        yield sprintf "watch[forum_threads]=%b" (Seq.contains WatchForumThreads w)
        yield sprintf "watch[critiques]=%b" (Seq.contains WatchCritiques w)
        yield sprintf "watch[scraps]=%b" (Seq.contains WatchScraps w)
        yield sprintf "watch[activity]=%b" (Seq.contains WatchActivity w)
        yield sprintf "watch[collections]=%b" (Seq.contains WatchCollections w)
    }

    let profileExtParams profileExtParams = seq {
        let c = Seq.cache profileExtParams
        yield sprintf "ext_collections=%b" (Seq.contains ExtCollections c)
        yield sprintf "ext_galleries=%b" (Seq.contains ExtGalleries c)
    }

    let profileModifications profileModifications = seq {
        let c = Seq.cache profileModifications
        for update in c do
            match update with
            | UserIsArtist v -> sprintf "user_is_artist=%b" v
            | ArtistLevel v -> sprintf "artist_level=%d" (int v)
            | ArtistSpecialty v -> sprintf "artist_specialty=%d" (int v)
            | Tagline v -> sprintf "tagline=%s" (Uri.EscapeDataString v)
            | Countryid v -> sprintf "countryid=%d" v
            | Website v -> sprintf "website=%s" (Uri.EscapeDataString v)
    }

    let embeddableStatusContent embeddableStatusContent = seq {
        match embeddableStatusContent.parent with
        | EmbeddableObjectParentStatus s -> yield sprintf "parentid=%O" s
        | NoEmbeddableObjectParent -> ()
        match embeddableStatusContent.object with
        | DeviationToEmbed s -> yield sprintf "id=%O" s
        | StatusToEmbed s -> yield sprintf "id=%O" s
        | NoEmbeddableObject -> ()
        match embeddableStatusContent.stash_item with
        | EmbeddableStashItem s -> yield sprintf "stashid=%O" s
        | NoEmbeddableStashItem -> ()
    }