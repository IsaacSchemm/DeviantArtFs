namespace DeviantArtFs.Api

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

    let AsyncPageByDeviantsYouWatch token limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/deviantsyouwatch"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<Deviation>>

    let AsyncGetByDeviantsYouWatch token batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPageByDeviantsYouWatch token batchsize)

    let AsyncGetMoreLikeThis token expansion seed =
        seq {
            yield sprintf "seed=%s" (Dafs.guid2str seed)
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/morelikethis/preview"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtMoreLikeThisPreviewResult>

    let AsyncPageNewest token expansion q limit offset =
        seq {
            match q with
            | SearchQuery s -> yield sprintf "q=%s" (Dafs.urlEncode s)
            | NoSearchQuery -> ()
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 120
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/newest"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtBrowsePagedResult>

    let AsyncGetNewest token expansion q batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPageNewest token expansion q batchsize)

    let AsyncPagePopular token expansion timerange q limit offset =
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
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 120
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/popular"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtBrowsePagedResult>

    let AsyncGetPopular token expansion timerange q batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPagePopular token expansion timerange q batchsize)

    let AsyncPagePostsByDeviantsYouWatch token limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/posts/deviantsyouwatch"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtPost>>

    let AsyncGetPostsByDeviantsYouWatch token batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPagePostsByDeviantsYouWatch token batchsize)

    let AsyncPageRecommended token expansion q limit offset =
        seq {
            match q with
            | SearchQuery s -> yield sprintf "q=%s" (Dafs.urlEncode s)
            | NoSearchQuery -> ()
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/recommended"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtRecommendedPagedResult>

    let AsyncGetRecommended token expansion q batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPageRecommended token expansion q batchsize)

    let AsyncPageTags token expansion tag limit offset =
        seq {
            yield sprintf "tag=%s" (Dafs.urlEncode tag)
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/tags"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtBrowsePagedResult>

    let AsyncGetTags token expansion tag batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPageTags token expansion tag batchsize)

    let AsyncSearchTags token tag_name =
        seq {
            yield sprintf "tag_name=%s" (Dafs.urlEncode tag_name)
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/tags/search"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtListOnlyResponse<DeviationTagSearchResult>>

    let AsyncPageTopic token expansion topic limit offset =
        seq {
            yield sprintf "topic=%s" (Dafs.urlEncode topic)
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 24
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/topic"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<Deviation>>

    let AsyncGetTopic token expansion topic batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPageTopic token expansion topic batchsize)

    let AsyncPageTopics token limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 10
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/topics"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtTopic>>

    let AsyncGetTopics token batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPageTopics token batchsize)

    let AsyncGetTopTopics token =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/toptopics"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtListOnlyResponse<DeviantArtTopic>>

    let AsyncPageUserJournals token expansion filter username limit offset =
        seq {
            yield sprintf "username=%s" (Dafs.urlEncode username)
            match filter with
            | NoUserJournalFilter -> yield "featured=0"
            | FeaturedJournalsOnly -> yield "featured=1"
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/user/journals"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<Deviation>>

    let AsyncGetUserJournals token expansion filter username batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPageUserJournals token expansion filter username batchsize)