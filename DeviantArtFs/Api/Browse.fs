namespace DeviantArtFs.Api

open DeviantArtFs
open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages

module Browse =
    let AsyncGetDailyDeviations token expansion date =
        seq {
            yield! QueryFor.dailyDeviationDate date
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/dailydeviations"
        |> Dafs.asyncRead
        |> Dafs.thenParse<ListOnlyResponse<Deviation>>

    let AsyncPageByDeviantsYouWatch token limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/deviantsyouwatch"
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<Deviation>>

    let AsyncGetByDeviantsYouWatch token batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPageByDeviantsYouWatch token batchsize)

    let AsyncGetMoreLikeThis token expansion seed =
        seq {
            yield sprintf "seed=%s" (Dafs.guid2str seed)
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/morelikethis/preview"
        |> Dafs.asyncRead
        |> Dafs.thenParse<MoreLikeThisPreviewResult>

    let AsyncPageNewest token expansion q limit offset =
        seq {
            yield! QueryFor.searchQuery q
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 120
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/newest"
        |> Dafs.asyncRead
        |> Dafs.thenParse<BrowsePage>

    let AsyncGetNewest token expansion q batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPageNewest token expansion q batchsize)

    let AsyncPagePopular token expansion timerange q limit offset =
        seq {
            yield! QueryFor.searchQuery q
            yield! QueryFor.timeRange timerange
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 120
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/popular"
        |> Dafs.asyncRead
        |> Dafs.thenParse<BrowsePage>

    let AsyncGetPopular token expansion timerange q batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPagePopular token expansion timerange q batchsize)

    let AsyncPagePostsByDeviantsYouWatch token limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/posts/deviantsyouwatch"
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<Post>>

    let AsyncGetPostsByDeviantsYouWatch token batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPagePostsByDeviantsYouWatch token batchsize)

    let AsyncPageRecommended token expansion q limit offset =
        seq {
            yield! QueryFor.searchQuery q
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/recommended"
        |> Dafs.asyncRead
        |> Dafs.thenParse<RecommendedPage>

    let AsyncGetRecommended token expansion q batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPageRecommended token expansion q batchsize)

    let AsyncPageTags token expansion tag limit offset =
        seq {
            yield sprintf "tag=%s" (System.Uri.EscapeDataString tag)
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/tags"
        |> Dafs.asyncRead
        |> Dafs.thenParse<BrowsePage>

    let AsyncGetTags token expansion tag batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPageTags token expansion tag batchsize)

    let AsyncSearchTags token tag_name =
        seq {
            yield sprintf "tag_name=%s" (System.Uri.EscapeDataString tag_name)
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/tags/search"
        |> Dafs.asyncRead
        |> Dafs.thenParse<ListOnlyResponse<TagSearchResult>>

    let AsyncPageTopic token expansion topic limit offset =
        seq {
            yield sprintf "topic=%s" (System.Uri.EscapeDataString topic)
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 24
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/topic"
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<Deviation>>

    let AsyncGetTopic token expansion topic batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPageTopic token expansion topic batchsize)

    let AsyncPageTopics token limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 10
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/topics"
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<Topic>>

    let AsyncGetTopics token batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPageTopics token batchsize)

    let AsyncGetTopTopics token =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/toptopics"
        |> Dafs.asyncRead
        |> Dafs.thenParse<ListOnlyResponse<Topic>>

    let AsyncPageUserJournals token expansion filter username limit offset =
        seq {
            yield sprintf "username=%s" (System.Uri.EscapeDataString username)
            yield! QueryFor.userJournalFilter filter
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/user/journals"
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<Deviation>>

    let AsyncGetUserJournals token expansion filter username batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPageUserJournals token expansion filter username batchsize)