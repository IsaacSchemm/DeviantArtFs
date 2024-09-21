namespace DeviantArtFs.Api

open DeviantArtFs
open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages
open System

module Browse =
    type DailyDeviationDate = DailyDeviationsToday | DailyDeviationsFor of DateTime with static member Default = DailyDeviationsToday

    let GetDailyDeviationsAsync token date =
        seq {
            match date with
            | DailyDeviationsFor d -> yield "date", d.ToString("YYYY-MM-dd")
            | DailyDeviationsToday -> ()
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

    let GetByDeviantsYouWatchAsync token batchsize offset = Utils.buildTaskSeq {
        initial_offset = offset
        get_page = (fun offset -> PageByDeviantsYouWatchAsync token batchsize offset)
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

    let MoreLikeThisPreviewAsync token seed =
        seq {
            yield "seed", Utils.guidString seed
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

    let PageTagsAsync token tag limit offset =
        seq {
            yield "tag", tag
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/tags"
        |> Utils.readAsync
        |> Utils.thenParse<BrowsePage>

    let GetTagsAsync token tag batchsize offset = Utils.buildTaskSeq {
        initial_offset = offset
        get_page = (fun offset -> PageTagsAsync token tag batchsize offset)
        extract_data = (fun page -> page.results)
        has_more = (fun page -> page.has_more)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
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

    let PageTopicAsync token topic limit offset =
        seq {
            yield "topic", topic
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 24
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/topic"
        |> Utils.readAsync
        |> Utils.thenParse<Page<Deviation>>

    let GetTopicAsync token topic batchsize offset = Utils.buildTaskSeq {
        initial_offset = offset
        get_page = (fun offset -> PageTopicAsync token topic batchsize offset)
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

    let PageTopicsAsync token limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 10
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/topics"
        |> Utils.readAsync
        |> Utils.thenParse<Page<Topic>>

    let GetTopicsAsync token batchsize offset = Utils.buildTaskSeq {
        initial_offset = offset
        get_page = (fun offset -> PageTopicsAsync token batchsize offset)
        extract_data = (fun page -> page.results.Value)
        has_more = (fun page -> page.has_more.Value)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
    }

    let GetTopTopicsAsync token =
        Seq.empty
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/browse/toptopics"
        |> Utils.readAsync
        |> Utils.thenParse<ListOnlyResponse<Topic>>
