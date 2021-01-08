namespace DeviantArtFs.Api

open System
open DeviantArtFs

module Browse =
    type DailyDeviationsRequest() = 
        member val Date = Nullable<DateTime>() with get, set

    let GetDailyDeviations token expansion (req: DailyDeviationsRequest) =
        seq {
            match Option.ofNullable req.Date with
            | Some d -> yield d.ToString("YYYY-MM-dd") |> sprintf "date=%s"
            | None -> ()
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/dailydeviations"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtListOnlyResponse<Deviation>>

    let PageDeviantsYouWatch token paging =
        seq {
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/deviantsyouwatch"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<Deviation>>

    let GetDeviantsYouWatch token offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (PageDeviantsYouWatch token)

    let GetMoreLikeThis token expansion (seed: Guid) =
        seq {
            yield sprintf "seed=%O" seed
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/morelikethis/preview"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtMoreLikeThisPreviewResult>

    type NewestRequest() =
        member val CategoryPath = null with get, set
        member val Q = null with get, set

    let PageNewest token expansion (req: NewestRequest) paging =
        seq {
            match Option.ofObj req.CategoryPath with
            | Some s -> yield sprintf "category_path=%s" (Dafs.urlEncode s)
            | None -> ()
            match Option.ofObj req.Q with
            | Some s -> yield sprintf "q=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 120
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/newest"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtBrowsePagedResult>

    let GetNewest token expansion req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (PageNewest token expansion req)

    type PopularTimeRange =
        | EightHours = 1
        | TwentyFourHours = 2
        | ThreeDays = 3
        | OneWeek = 4
        | OneMonth = 5
        | AllTime = 6

    type PopularRequest() =
        member val CategoryPath = null with get, set
        member val Q = null with get, set
        member val Timerange = PopularTimeRange.TwentyFourHours with get, set

    let PagePopular token expansion (req: PopularRequest) paging =
        seq {
            match Option.ofObj req.CategoryPath with
            | Some s -> yield sprintf "category_path=%s" (Dafs.urlEncode s)
            | None -> ()
            match Option.ofObj req.Q with
            | Some s -> yield sprintf "q=%s" (Dafs.urlEncode s)
            | None -> ()
            match req.Timerange with
            | PopularTimeRange.EightHours -> yield "timerange=8hr"
            | PopularTimeRange.TwentyFourHours -> yield "timerange=24hr"
            | PopularTimeRange.ThreeDays -> yield "timerange=3days"
            | PopularTimeRange.OneWeek -> yield "timerange=1week"
            | PopularTimeRange.OneMonth -> yield "timerange=1month"
            | PopularTimeRange.AllTime -> yield "timerange=alltime"
            | _ -> ()
            yield! QueryFor.paging paging 120
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/popular"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtBrowsePagedResult>

    let GetPopular token expansion req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (PagePopular token expansion req)

    let PagePostsByDeviantsYouWatch token paging =
        seq {
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/posts/deviantsyouwatch"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtPost>>

    let GetPostsByDeviantsYouWatch token offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (PagePostsByDeviantsYouWatch token)

    type RecommendedRequest() =
         member val Q = null with get, set

    let PageRecommended token expansion (req: RecommendedRequest) paging =
        seq {
            match Option.ofObj req.Q with
            | Some s -> yield sprintf "q=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/recommended"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtRecommendedPagedResult>

    let GetRecommended token expansion req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (PageRecommended token expansion req)

    let PageTags token expansion (tag: string) paging =
        seq {
            yield sprintf "tag=%s" (Dafs.urlEncode tag)
            yield! QueryFor.paging paging 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/tags"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtBrowsePagedResult>

    let GetTags token expansion tag offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (PageTags token expansion tag)

    let SearchTags token (tag_name: string) =
        seq {
            yield sprintf "tag_name=%s" (Dafs.urlEncode tag_name)
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/tags/search"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtListOnlyResponse<DeviationTagSearchResult>>

    let PageTopic token expansion (topic: string) paging =
        seq {
            match Option.ofObj topic with
            | Some s -> yield sprintf "topic=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 24
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/topic"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<Deviation>>

    let GetTopic token expansion topic offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (PageTopic token expansion topic)

    type TopicsRequest() =
        member val NumDeviationsPerTopic = Nullable() with get, set

    let PageTopics token (req: TopicsRequest) paging =
        seq {
            match Option.ofNullable req.NumDeviationsPerTopic with
            | Some s -> yield sprintf "topic=%d" s
            | None -> ()
            yield! QueryFor.paging paging 10
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/topics"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtTopic>>

    let GetTopics token req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (PageTopics token req)

    let GetTopTopics token =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/toptopics"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtListOnlyResponse<DeviantArtTopic>>

    type UserJournalsRequest(username: string) =
        member __.Username = username
        member val Featured = true with get, set

    let PageUserJournals token expansion (req: UserJournalsRequest) paging =
        seq {
            yield sprintf "username=%s" (Dafs.urlEncode req.Username)
            yield sprintf "featured=%b" req.Featured
            yield! QueryFor.paging paging 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/browse/user/journals"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<Deviation>>

    let GetUserJournals token expansion req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (PageUserJournals token expansion req)