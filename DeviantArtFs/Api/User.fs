namespace DeviantArtFs.Api

open DeviantArtFs
open System

module User =
    let AsyncGetMessagingNetworkToken token =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/user/damntoken"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtMessagingNetworkToken>

    type FriendsRequest() =
        member val Username: string = null with get, set

    let AsyncPageFriends token expansion (req: FriendsRequest) paging =
        seq {
            yield! QueryFor.paging paging 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/%s" (Dafs.urlEncode req.Username))
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtFriendRecord>>

    let AsyncGetFriends token expansion req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageFriends token expansion req)

    type FriendsSearchRequest(query: string) =
        member __.Query = query
        member val Username = null with get, set

    let AsyncSearchFriends token (req: FriendsSearchRequest) =
        seq {
            yield req.Query |> Dafs.urlEncode |> sprintf "query=%s"
            if req.Username |> isNull |> not then
                yield req.Username |> Dafs.urlEncode |> sprintf "username=%s"
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/user/friends/search"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtListOnlyResponse<DeviantArtUser>>
        
    type WatchRequest(username: string) =
        member __.Username = username
        member val Friend = true with get, set
        member val Deviations = true with get, set
        member val Journals = true with get, set
        member val ForumThreads = true with get, set
        member val Critiques = true with get, set
        member val Scraps = false with get, set
        member val Activity = true with get, set
        member val Collections = true with get, set

    let AsyncWatch token (ps: WatchRequest) =
        seq {
            yield sprintf "watch[friend]=%b" ps.Friend
            yield sprintf "watch[deviations]=%b" ps.Deviations
            yield sprintf "watch[journals]=%b" ps.Journals
            yield sprintf "watch[forum_threads]=%b" ps.ForumThreads
            yield sprintf "watch[critiques]=%b" ps.Critiques
            yield sprintf "watch[scraps]=%b" ps.Scraps
            yield sprintf "watch[activity]=%b" ps.Activity
            yield sprintf "watch[collections]=%b" ps.Collections
        }
        |> Dafs.createRequest Dafs.Method.POST token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/watch/%s" (Dafs.urlEncode ps.Username))
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>

    let AsyncUnwatch token username =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/unwatch/%s" (Dafs.urlEncode username))
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>

    let AsyncGetWatching token username =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/watching/%s" username)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtWatchingResponse>

    type ProfileRequest(username: string) =
        member __.Username = username
        member val ExtCollections = false with get, set
        member val ExtGalleries = false with get, set

    let AsyncGetProfile token expansion (req: ProfileRequest) =
        seq {
            yield sprintf "ext_collections=%b" req.ExtCollections
            yield sprintf "ext_galleries=%b" req.ExtGalleries
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/profile/%s" (Dafs.urlEncode req.Username))
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtProfile>

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

    [<RequireQualifiedAccess>]
    type ProfileUpdateField =
        | UserIsArtist of bool
        | ArtistLevel of ArtistLevel
        | ArtistSpecialty of ArtistSpecialty
        | Tagline of string
        | Countryid of int
        | Website of string

    let AsyncUpdateProfile token (updates: ProfileUpdateField seq) =
        seq {
            for update in updates do
                match update with
                | ProfileUpdateField.UserIsArtist v -> sprintf "user_is_artist=%b" v
                | ProfileUpdateField.ArtistLevel v -> sprintf "artist_level=%s" (v.ToString "d")
                | ProfileUpdateField.ArtistSpecialty v -> sprintf "artist_specialty=%s" (v.ToString "d")
                | ProfileUpdateField.Tagline v -> sprintf "tagline=%s" (Dafs.urlEncode v)
                | ProfileUpdateField.Countryid v -> sprintf "countryid=%d" v
                | ProfileUpdateField.Website v -> sprintf "website=%s" (Dafs.urlEncode v)
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/user/profile/update"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>

    let AsyncGetStatus token (id: Guid) =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/statuses/%O" id)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtStatus>

    let AsyncPageStatuses token (username: string) paging =
        seq {
            yield sprintf "username=%s" (Dafs.urlEncode username)
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/user/statuses"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtStatus>>

    let AsyncGetStatuses token username offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageStatuses token username)

    type StatusPostRequest(body: string) =
        member __.Body = body
        member val Statusid = Nullable<Guid>() with get, set
        member val Parentid = Nullable<Guid>() with get, set
        member val Stashid = Nullable<int64>() with get, set

    let AsyncPostStatus token (ps: StatusPostRequest) =
        seq {
            match Option.ofObj ps.Body with
            | Some s -> yield sprintf "body=%s" (Dafs.urlEncode s)
            | None -> ()
            match Option.ofNullable ps.Parentid with
            | Some s -> yield sprintf "parentid=%O" s
            | None -> ()
            match Option.ofNullable ps.Statusid with
            | Some s -> yield sprintf "id=%O" s
            | None -> ()
            match Option.ofNullable ps.Stashid with
            | Some s -> yield sprintf "stashid=%O" s
            | None -> ()
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/user/statuses/post"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtStatusPostResponse>

    type WatchersRequest() =
        member val Username: string = null with get, set

    let AsyncPageWatchers token expansion (req: FriendsRequest) paging =
        seq {
            yield! QueryFor.paging paging 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/watchers/%s" (Dafs.urlEncode req.Username))
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtWatcherRecord>>

    let AsyncGetWatchers token expansion req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageWatchers token expansion req)

    let AsyncWhoami token expansion =
        seq {
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/user/whoami"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtUser>

    let AsyncWhois token expansion usernames =
        seq {
            for u in usernames do
                yield u |> Dafs.urlEncode |> sprintf "usernames[]=%s"
                yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/user/whois"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtListOnlyResponse<DeviantArtUser>>