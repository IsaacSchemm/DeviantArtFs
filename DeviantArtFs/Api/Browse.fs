namespace DeviantArtFs.Api

open DeviantArtFs
open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages
open System

module Browse =
    type DailyDeviationDate =
    | DailyDeviationsToday
    | DailyDeviationsFor of DateTime

    let AsyncGetDailyDeviations token date =
        seq {
            match date with
            | DailyDeviationsFor d -> yield "date", d.ToString("YYYY-MM-dd")
            | DailyDeviationsToday -> ()
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/dailydeviations"
        |> Utils.asyncRead
        |> Utils.thenParse<ListOnlyResponse<Deviation>>

    let AsyncPageByDeviantsYouWatch token limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/deviantsyouwatch"
        |> Utils.asyncRead
        |> Utils.thenParse<Page<Deviation>>

    let GetByDeviantsYouWatchAsync token batchsize offset = Utils.buildAsyncSeq {
        initial_offset = offset
        get_page = (fun offset -> AsyncPageByDeviantsYouWatch token batchsize offset)
        extract_data = (fun page -> page.results.Value)
        has_more = (fun page -> page.has_more.Value)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
    }

    let AsyncPageHome token limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/home"
        |> Utils.asyncRead
        |> Utils.thenParse<Page<Deviation>>

    let GetHomeAsync token batchsize offset = Utils.buildAsyncSeq {
        initial_offset = offset
        get_page = (fun offset -> AsyncPageHome token batchsize offset)
        extract_data = (fun page -> page.results.Value)
        has_more = (fun page -> page.has_more.Value)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
    }

    type SuggestedCollection = {
        collection: Gallection
        deviations: Deviation list
    }

    type MoreLikeThisPreviewResult = {
        seed: Guid
        author: User
        more_from_artist: Deviation list
        more_from_da: Deviation list
        suggested_collections: SuggestedCollection list option
    }

    let AsyncMoreLikeThisPreview token seed =
        seq {
            yield "seed", Utils.guidString seed
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/morelikethis/preview"
        |> Utils.asyncRead
        |> Utils.thenParse<MoreLikeThisPreviewResult>

    type SearchQuery =
    | NoSearchQuery
    | SearchQuery of string

    type BrowsePage = {
        has_more: bool
        next_offset: int option
        error_code: int option
        estimated_total: int option
        results: Deviation list
    }

    let AsyncPageTags token tag limit offset =
        seq {
            yield "tag", tag
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/tags"
        |> Utils.asyncRead
        |> Utils.thenParse<BrowsePage>

    let GetTagsAsync token tag batchsize offset = Utils.buildAsyncSeq {
        initial_offset = offset
        get_page = (fun offset -> AsyncPageTags token tag batchsize offset)
        extract_data = (fun page -> page.results)
        has_more = (fun page -> page.has_more)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
    }

    type TagSearchResult = {
        tag_name: string
    }

    let AsyncSearchTags token tag_name =
        seq {
            yield "tag_name", tag_name
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/tags/search"
        |> Utils.asyncRead
        |> Utils.thenParse<ListOnlyResponse<TagSearchResult>>

    let AsyncPageTopic token topic limit offset =
        seq {
            yield "topic", topic
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 24
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/topic"
        |> Utils.asyncRead
        |> Utils.thenParse<Page<Deviation>>

    let GetTopicAsync token topic batchsize offset = Utils.buildAsyncSeq {
        initial_offset = offset
        get_page = (fun offset -> AsyncPageTopic token topic batchsize offset)
        extract_data = (fun page -> page.results.Value)
        has_more = (fun page -> page.has_more.Value)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
    }

    type Topic = {
        name: string
        canonical_name: string
        example_deviations: Deviation list option
        deviations: Deviation list option
    }

    let AsyncPageTopics token limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 10
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/topics"
        |> Utils.asyncRead
        |> Utils.thenParse<Page<Topic>>

    let GetTopicsAsync token batchsize offset = Utils.buildAsyncSeq {
        initial_offset = offset
        get_page = (fun offset -> AsyncPageTopics token batchsize offset)
        extract_data = (fun page -> page.results.Value)
        has_more = (fun page -> page.has_more.Value)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
    }

    let AsyncGetTopTopics token =
        Seq.empty
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/toptopics"
        |> Utils.asyncRead
        |> Utils.thenParse<ListOnlyResponse<Topic>>
