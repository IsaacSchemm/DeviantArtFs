namespace DeviantArtFs.Api

open System
open DeviantArtFs
open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages
open FSharp.Control

module User =
    type MessagingNetworkToken = {
        damntoken: string
    }

    let GetMessagingNetworkTokenAsync token =
        Seq.empty
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/user/damntoken"
        |> Utils.readAsync
        |> Utils.thenParse<MessagingNetworkToken>

    type WatchInfo = {
        friend: bool
        deviations: bool
        journals: bool
        forum_threads: bool
        critiques: bool
        scraps: bool
        activity: bool
        collections: bool
    }

    type FriendRecord = {
        user: User
        is_watching: bool
        watches_you: bool
        lastvisit: DateTimeOffset option
        watch: WatchInfo
    }

    type WatcherRecord = {
        user: User
        is_watching: bool
        lastvisit: DateTimeOffset option
        watch: WatchInfo
    }

    let PageFriendsAsync token user limit offset =
        let username = match user with | ForUser u -> u | ForCurrentUser -> ""

        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/user/friends/{Uri.EscapeDataString username}"
        |> Utils.readAsync
        |> Utils.thenParse<Page<FriendRecord>>

    let GetFriendsAsync token req batchsize offset = Utils.buildAsyncSeq {
        get_page = (fun offset -> PageFriendsAsync token req batchsize offset)
        extract_data = (fun page -> page.results.Value)
        has_more = (fun page -> page.has_more.Value)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
        initial_offset = offset
    }

    type AdditionalUser = AdditionalUser of string | NoAdditionalUser

    let SearchFriendsAsync token user additionalUser q =
        seq {
            yield! QueryFor.userScope user
            match additionalUser with
            | AdditionalUser u -> yield "search", u
            | NoAdditionalUser -> ()
            yield "q", q
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/user/friends/search"
        |> Utils.readAsync
        |> Utils.thenParse<ListOnlyResponse<User>>

    type WatchOptions = {
        friend: bool
        deviations: bool
        journals: bool
        forum_threads: bool
        critiques: bool
        scraps: bool
        activity: bool
        collections: bool
    } with
        static member None = {
            friend = false
            deviations = false
            journals = false
            forum_threads = false
            critiques = false
            scraps = false
            activity = false
            collections = false
        }
        static member All = {
            friend = true
            deviations = true
            journals = true
            forum_threads = true
            critiques = true
            scraps = true
            activity = true
            collections = true
        }

    let WatchAsync token watchTypes username =
        seq {
            yield "watch[friend]", if watchTypes.friend then "1" else "0"
            yield "watch[deviations]", if watchTypes.deviations then "1" else "0"
            yield "watch[journals]", if watchTypes.journals then "1" else "0"
            yield "watch[forum_threads]", if watchTypes.forum_threads then "1" else "0"
            yield "watch[critiques]", if watchTypes.critiques then "1" else "0"
            yield "watch[scraps]", if watchTypes.scraps then "1" else "0"
            yield "watch[activity]", if watchTypes.activity then "1" else "0"
            yield "watch[collections]", if watchTypes.collections then "1" else "0"
        }
        |> Utils.post token $"https://www.deviantart.com/api/v1/oauth2/user/friends/watch/{Uri.EscapeDataString username}"
        |> Utils.readAsync
        |> Utils.thenParse<SuccessOrErrorResponse>

    let UnwatchAsync token username =
        Seq.empty
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/user/friends/unwatch/{Uri.EscapeDataString username}"
        |> Utils.readAsync
        |> Utils.thenParse<SuccessOrErrorResponse>

    type WatchingResponse = {
        watching: bool
    }

    let GetWatchingAsync token username =
        Seq.empty
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/user/friends/watching/{username}"
        |> Utils.readAsync
        |> Utils.thenParse<WatchingResponse>

    type ProfileExtParams = {
        ext_collections: bool
        ext_galleries: bool
    } with
        static member None = { ext_collections = false; ext_galleries = false }
        static member All = { ext_collections = true; ext_galleries = true }

    type ProfileStats = {
        user_deviations: int
        user_favourites: int
        user_comments: int
        profile_pageviews: int
        profile_comments: int
    }

    type Profile = {
        user: User
        is_watching: bool
        profile_url: string
        user_is_artist: bool
        artist_level: string option
        artist_specialty: string option
        real_name: string
        tagline: string
        countryid: int
        country: string
        website: string
        bio: string
        cover_photo: string option
        profile_pic: Deviation option
        last_status: Status option
        stats: ProfileStats
        collections: CollectionFolder list option
        galleries: GalleryFolder list option
    }

    let GetProfileAsync token profile_ext_params user =
        let username = match user with | ForUser u -> u | ForCurrentUser -> ""

        seq {
            yield "ext_collections", if profile_ext_params.ext_collections then "1" else "0"
            yield "ext_galleries", if profile_ext_params.ext_galleries then "1" else "0"
        }
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/user/profile/{Uri.EscapeDataString username}"
        |> Utils.readAsync
        |> Utils.thenParse<Profile>

    type ProfilePostsPage = {
        has_more: bool
        next_cursor: string option
        prev_cursor: string option
        results: Deviation list
    }

    type ProfilePostsCursor = ProfilePostsCursor of string | FromBeginning

    let PageProfilePostsAsync token username cursor =
        seq {
            yield "username", username
            match cursor with
            | ProfilePostsCursor c -> yield "cursor", c
            | FromBeginning -> ()
        }
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/user/profile/posts"
        |> Utils.readAsync
        |> Utils.thenParse<ProfilePostsPage>

    let GetProfilePostsAsync token username cursor = Utils.buildAsyncSeq {
        get_page = (fun cursor -> PageProfilePostsAsync token username cursor)
        extract_data = (fun page -> page.results)
        has_more = (fun page -> page.has_more)
        extract_next_offset = (fun page -> ProfilePostsCursor page.next_cursor.Value)
        initial_offset = cursor
    }

    type Interest =
    | FavoriteVisualArtist = 1
    | FavoriteMovies = 2
    | FavoriteTVShows = 3
    | FavoriteBands = 4
    | FavoriteBooks = 5
    | FavoriteWriters = 6
    | FavoriteGames = 7
    | FavoriteGamingPlatform = 8
    | ToolsOfTheTrade = 9
    | OtherInterests = 10

    type Social =
    | Behance = 1
    | Dribble = 2
    | Etsy = 3
    | Facebook = 4
    | Flickr = 5
    | GooglePlus = 6
    | Instagram = 7
    | LinkedIn = 8
    | Patreon = 9
    | Pinterest = 10
    | Tumblr = 11
    | Twitch = 12
    | Twitter = 13
    | Vimeo = 14
    | YouTube = 15

    type ArtistLevel =
    | None=0
    | Student=1
    | Hobbyist=2
    | Professional=3

    type ArtistSpecialty =
    | None=0
    | ArtisanCrafts = 1
    | DesignAndInterfaces = 2
    | DigitalArt = 3
    | FilmAndAnimation = 4
    | Literature = 5
    | Photography = 6
    | TraditionalArt = 7
    | Other = 8
    | Varied = 9

    type ProfileModification =
    | UserIsArtist of bool
    | ArtistLevel of ArtistLevel
    | ArtistSpecialty of ArtistSpecialty
    | Countryid of int
    | Website of string
    | WebsiteLabel of string
    | Tagline of string
    | ShowBadges of bool
    | Interest of Interest * string
    | Social of Social * string

    let UpdateProfileAsync token modifications =
        seq {
            for m in modifications do
                match m with
                | UserIsArtist v -> "user_is_artist", string v
                | ArtistLevel v -> "artist_level", string (int v)
                | ArtistSpecialty v -> "artist_specialty", string (int v)
                | Countryid v -> "countryid", string v
                | Website v -> "website", v
                | WebsiteLabel v -> "website_label", v
                | Tagline v -> "tagline", v
                | ShowBadges v -> "show_badges", if v then "1" else "0"
                | Interest (e, v) -> $"interests[{int e}]", v
                | Social (e, v) -> $"social_links[{int e}]", v
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/user/profile/update"
        |> Utils.readAsync
        |> Utils.thenParse<SuccessOrErrorResponse>

    let GetStatusAsync token id =
        Seq.empty
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/user/statuses/{Utils.guidString id}"
        |> Utils.readAsync
        |> Utils.thenParse<Status>

    type EmbeddableObject = Deviation of Guid | Status of Guid | Nothing
    type EmbeddableObjectParent = ParentStatus of Guid | NoParent
    type EmbeddableStashItem = StashItem of int64 | NoStashItem

    type EmbeddableStatusContent = {
        object: EmbeddableObject
        parent: EmbeddableObjectParent
        stash_item: EmbeddableStashItem
    } with
        static member None = {
            object = Nothing
            parent = NoParent
            stash_item = NoStashItem
        }

    type StatusPostResponse = {
        statusid: Guid
    }

    let PostStatusAsync token embeddable_content body =
        seq {
            match Option.ofObj body with
            | Some s -> yield "body", s
            | None -> ()
            match embeddable_content.parent with
            | ParentStatus s -> yield "parentid", string s
            | NoParent -> ()
            match embeddable_content.object with
            | Deviation s -> yield "id", string s
            | Status s -> yield "id", string s
            | Nothing -> ()
            match embeddable_content.stash_item with
            | StashItem s -> yield "stashid", string s
            | NoStashItem -> ()
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/user/statuses/post"
        |> Utils.readAsync
        |> Utils.thenParse<StatusPostResponse>

    let GetTiersAsync token username =
        Seq.empty
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/user/tiers/{Uri.EscapeDataString username}"
        |> Utils.readAsync
        |> Utils.thenParse<ListOnlyResponse<Deviation>>

    let PageWatchersAsync token user limit offset =
        let username = match user with | ForUser u -> u | ForCurrentUser -> ""

        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/user/watchers/{Uri.EscapeDataString username}"
        |> Utils.readAsync
        |> Utils.thenParse<Page<WatcherRecord>>

    let GetWatchersAsync token user limit offset = Utils.buildAsyncSeq {
        get_page = (fun offset -> PageWatchersAsync token user limit offset)
        extract_data = (fun page -> page.results.Value)
        has_more = (fun page -> page.has_more.Value)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
        initial_offset = offset
    }

    let WhoamiAsync token =
        Seq.empty
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/user/whoami"
        |> Utils.readAsync
        |> Utils.thenParse<User>

    let WhoisAsync token usernames =
        seq {
            for u in usernames do
                yield "usernames[]", u
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/user/whois"
        |> Utils.readAsync
        |> Utils.thenParse<ListOnlyResponse<User>>