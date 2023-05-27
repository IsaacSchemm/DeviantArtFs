namespace DeviantArtFs.Api

open DeviantArtFs
open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages
open FSharp.Control
open System

module Browse =
    type DailyDeviationDate = DailyDeviationsToday | DailyDeviationsFor of DateTime with static member Default = DailyDeviationsToday

    let GetDailyDeviationsAsync token expansion date =
        seq {
            match date with
            | DailyDeviationsFor d -> yield "date", d.ToString("YYYY-MM-dd")
            | DailyDeviationsToday -> ()
            yield! QueryFor.objectExpansion expansion
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/dailydeviations"
        |> Utils.readAsync
        |> Utils.thenParse<ListOnlyResponse<Deviation>>

    let PageByDeviantsYouWatchAsync token limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/deviantsyouwatch"
        |> Utils.readAsync
        |> Utils.thenParse<Page<Deviation>>

    let GetByDeviantsYouWatchAsync token batchsize offset = taskSeq {
        let mutable offset = offset
        let mutable has_more = true
        while has_more do
            let! data = PageByDeviantsYouWatchAsync token batchsize offset
            yield! data.results.Value
            has_more <- data.has_more.Value
            if has_more then
                offset <- PagingOffset data.next_offset.Value
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

    let MoreLikeThisPreviewAsync token expansion seed =
        seq {
            yield "seed", Utils.guidString seed
            yield! QueryFor.objectExpansion expansion
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/morelikethis/preview"
        |> Utils.readAsync
        |> Utils.thenParse<MoreLikeThisPreviewResult>

    type SearchQuery = NoSearchQuery | SearchQuery of string with static member Default = NoSearchQuery

    type BrowsePage = {
        has_more: bool
        next_offset: int option
        error_code: int option
        estimated_total: int option
        results: Deviation list
    }

    let PageNewestAsync token expansion q limit offset =
        seq {
            match q with
            | SearchQuery s -> yield "q", s
            | NoSearchQuery -> ()
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 120
            yield! QueryFor.objectExpansion expansion
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/newest"
        |> Utils.readAsync
        |> Utils.thenParse<BrowsePage>

    let GetNewestAsync token expansion q batchsize offset = taskSeq {
        let mutable offset = offset
        let mutable has_more = true
        while has_more do
            let! data = PageNewestAsync token expansion q batchsize offset
            yield! data.results
            has_more <- data.has_more
            if has_more then
                offset <- PagingOffset data.next_offset.Value
    }

    type PopularTimeRange = Unspecified | Now | OneWeek | OneMonth | AllTime with static member Default = Unspecified

    let PagePopularAsync token expansion timerange q limit offset =
        seq {
            match q with
            | SearchQuery s -> yield "q", s
            | NoSearchQuery -> ()
            match timerange with
            | Now -> yield "timerange", "now"
            | OneWeek -> yield "timerange", "1week"
            | OneMonth -> yield "timerange", "1month"
            | AllTime -> yield "timerange", "alltime"
            | Unspecified -> ()
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 120
            yield! QueryFor.objectExpansion expansion
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/popular"
        |> Utils.readAsync
        |> Utils.thenParse<BrowsePage>

    let GetPopularAsync token expansion timerange q batchsize offset = taskSeq {
        let mutable offset = offset
        let mutable has_more = true
        while has_more do
            let! data = PagePopularAsync token expansion timerange q batchsize offset
            yield! data.results
            has_more <- data.has_more
            if has_more then
                offset <- PagingOffset data.next_offset.Value
    }

    type Post = {
        journal: Deviation option
        status: Status option
    }

    let PagePostsByDeviantsYouWatchAsync token expansion limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/posts/deviantsyouwatch"
        |> Utils.readAsync
        |> Utils.thenParse<Page<Post>>

    let GetPostsByDeviantsYouWatchAsync token expansion batchsize offset = taskSeq {
        let mutable offset = offset
        let mutable has_more = true
        while has_more do
            let! data = PagePostsByDeviantsYouWatchAsync token expansion batchsize offset
            yield! data.results.Value
            has_more <- data.has_more.Value
            if has_more then
                offset <- PagingOffset data.next_offset.Value
    }

    type RecommendedPage = {
        has_more: bool
        next_offset: int option
        estimated_total: int option
        results: Deviation list
    }

    let PageRecommendedAsync token expansion q limit offset =
        seq {
            match q with
            | SearchQuery s -> yield "q", s
            | NoSearchQuery -> ()
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/recommended"
        |> Utils.readAsync
        |> Utils.thenParse<RecommendedPage>

    let GetRecommendedAsync token expansion q batchsize offset = taskSeq {
        let mutable offset = offset
        let mutable has_more = true
        while has_more do
            let! data = PageRecommendedAsync token expansion q batchsize offset
            yield! data.results
            has_more <- data.has_more
            if has_more then
                offset <- PagingOffset data.next_offset.Value
    }

    let PageTagsAsync token expansion tag limit offset =
        seq {
            yield "tag", tag
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/tags"
        |> Utils.readAsync
        |> Utils.thenParse<BrowsePage>

    let GetTagsAsync token expansion tag batchsize offset = taskSeq {
        let mutable offset = offset
        let mutable has_more = true
        while has_more do
            let! data = PageTagsAsync token expansion tag batchsize offset
            yield! data.results
            has_more <- data.has_more
            if has_more then
                offset <- PagingOffset data.next_offset.Value
    }

    type TagSearchResult = {
        tag_name: string
    }

    let SearchTagsAsync token tag_name =
        seq {
            yield "tag_name", tag_name
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/tags/search"
        |> Utils.readAsync
        |> Utils.thenParse<ListOnlyResponse<TagSearchResult>>

    let PageTopicAsync token expansion topic limit offset =
        seq {
            yield "topic", topic
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 24
            yield! QueryFor.objectExpansion expansion
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/topic"
        |> Utils.readAsync
        |> Utils.thenParse<Page<Deviation>>

    let GetTopicAsync token expansion topic batchsize offset = taskSeq {
        let mutable offset = offset
        let mutable has_more = true
        while has_more do
            let! data = PageTopicAsync token expansion topic batchsize offset
            yield! data.results.Value
            has_more <- data.has_more.Value
            if has_more then
                offset <- PagingOffset data.next_offset.Value
    }

    type Topic = {
        name: string
        canonical_name: string
        example_deviations: Deviation list option
        deviations: Deviation list option
    }

    let PageTopicsAsync token limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 10
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/topics"
        |> Utils.readAsync
        |> Utils.thenParse<Page<Topic>>

    let GetTopicsAsync token batchsize offset = taskSeq {
        let mutable offset = offset
        let mutable has_more = true
        while has_more do
            let! data = PageTopicsAsync token batchsize offset
            yield! data.results.Value
            has_more <- data.has_more.Value
            if has_more then
                offset <- PagingOffset data.next_offset.Value
    }

    let GetTopTopicsAsync token =
        Seq.empty
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/toptopics"
        |> Utils.readAsync
        |> Utils.thenParse<ListOnlyResponse<Topic>>

    type UserJournalFilter = NoUserJournalFilter | FeaturedJournalsOnly with static member Default = FeaturedJournalsOnly

    let PageUserJournalsAsync token expansion filter username limit offset =
        seq {
            yield "username", username
            match filter with
            | NoUserJournalFilter -> yield "featured", "0"
            | FeaturedJournalsOnly -> yield "featured", "1"
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/user/journals"
        |> Utils.readAsync
        |> Utils.thenParse<Page<Deviation>>

    let GetUserJournalsAsync token expansion filter username batchsize offset = taskSeq {
        let mutable offset = offset
        let mutable has_more = true
        while has_more do
            let! data = PageUserJournalsAsync token expansion filter username batchsize offset
            yield! data.results.Value
            has_more <- data.has_more.Value
            if has_more then
                offset <- PagingOffset data.next_offset.Value
    }