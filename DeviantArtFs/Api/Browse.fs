namespace DeviantArtFs.Api

open System
open DeviantArtFs
open DeviantArtFs.ParameterTypes

module Browse =
    let AsyncGetDailyDeviations token expansion date =
        seq {
            match date with
            | DailyDeviationsFor d -> yield d.ToString("YYYY-MM-dd") |> sprintf "date=%s"
            | DailyDeviationsToday -> ()
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/dailydeviations"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtListOnlyResponse<Deviation>>

    let AsyncPageByDeviantsYouWatch token paging =
        seq {
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/deviantsyouwatch"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<Deviation>>

    let AsyncGetByDeviantsYouWatch token offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageByDeviantsYouWatch token)

    let AsyncGetMoreLikeThis token expansion (seed: Guid) =
        seq {
            yield sprintf "seed=%O" seed
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/morelikethis/preview"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtMoreLikeThisPreviewResult>

    let AsyncPageNewest token expansion q paging =
        seq {
            match q with
            | SearchQuery s -> yield sprintf "q=%s" (Dafs.urlEncode s)
            | NoSearchQuery -> ()
            yield! QueryFor.paging paging 120
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/newest"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtBrowsePagedResult>

    let AsyncGetNewest token expansion q offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageNewest token expansion q)

    let AsyncPagePopular token expansion timerange q paging =
        seq {
            match q with
            | SearchQuery s -> yield sprintf "q=%s" (Dafs.urlEncode s)
            | NoSearchQuery -> ()
            match timerange with
            | PopularNow -> yield "timerange=now"
            | PopularOneWeek -> yield "timerange=1week"
            | PopularOneMonth -> yield "timerange=1month"
            | PopularAllTime -> yield "timerange=alltime"
            | UnspecifiedPopularTimeRange -> ()
            yield! QueryFor.paging paging 120
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/popular"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtBrowsePagedResult>

    let AsyncGetPopular token expansion timerange q offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPagePopular token expansion timerange q)

    let AsyncPagePostsByDeviantsYouWatch token paging =
        seq {
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/posts/deviantsyouwatch"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtPost>>

    let AsyncGetPostsByDeviantsYouWatch token offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPagePostsByDeviantsYouWatch token)

    let AsyncPageRecommended token expansion q paging =
        seq {
            match q with
            | SearchQuery s -> yield sprintf "q=%s" (Dafs.urlEncode s)
            | NoSearchQuery -> ()
            yield! QueryFor.paging paging 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/recommended"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtRecommendedPagedResult>

    let AsyncGetRecommended token expansion q offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageRecommended token expansion q)

    let AsyncPageTags token expansion tag paging =
        seq {
            yield sprintf "tag=%s" (Dafs.urlEncode tag)
            yield! QueryFor.paging paging 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/tags"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtBrowsePagedResult>

    let AsyncGetTags token expansion tag offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageTags token expansion tag)

    let AsyncSearchTags token tag_name =
        seq {
            yield sprintf "tag_name=%s" (Dafs.urlEncode tag_name)
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/tags/search"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtListOnlyResponse<DeviationTagSearchResult>>

    let AsyncPageTopic token expansion topic paging =
        seq {
            yield sprintf "topic=%s" (Dafs.urlEncode topic)
            yield! QueryFor.paging paging 24
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/topic"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<Deviation>>

    let AsyncGetTopic token expansion topic offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageTopic token expansion topic)

    let AsyncPageTopics token paging =
        seq {
            yield! QueryFor.paging paging 10
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/topics"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtTopic>>

    let AsyncGetTopics token offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageTopics token)

    let AsyncGetTopTopics token =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/toptopics"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtListOnlyResponse<DeviantArtTopic>>

    let AsyncPageUserJournals token expansion filter username paging =
        seq {
            yield sprintf "username=%s" (Dafs.urlEncode username)
            match filter with
            | NoUserJournalFilter -> yield "featured=0"
            | FeaturedJournalsOnly -> yield "featured=1"
            yield! QueryFor.paging paging 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/user/journals"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<Deviation>>

    let AsyncGetUserJournals token expansion filter username offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageUserJournals token expansion filter username)