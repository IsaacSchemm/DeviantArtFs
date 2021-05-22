namespace DeviantArtFs.Api

open System
open DeviantArtFs
open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages

module User =
    let AsyncGetMessagingNetworkToken token =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/user/damntoken"
        |> Dafs.asyncRead
        |> Dafs.thenParse<MessagingNetworkToken>

    type FriendsRequest() =
        member val Username: string = null with get, set

    let AsyncPageFriends token expansion (req: FriendsRequest) limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/%s" (Uri.EscapeDataString req.Username))
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<FriendRecord>>

    let AsyncGetFriends token expansion req batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPageFriends token expansion req batchsize)

    type FriendsSearchRequest(query: string) =
        member __.Query = query
        member val Username = null with get, set

    let AsyncSearchFriends token (req: FriendsSearchRequest) =
        seq {
            yield req.Query |> Uri.EscapeDataString |> sprintf "query=%s"
            if req.Username |> isNull |> not then
                yield req.Username |> Uri.EscapeDataString |> sprintf "username=%s"
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/user/friends/search"
        |> Dafs.asyncRead
        |> Dafs.thenParse<ListOnlyResponse<User>>
        
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
        |> Dafs.createRequest Dafs.Method.POST token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/watch/%s" (Uri.EscapeDataString ps.Username))
        |> Dafs.asyncRead
        |> Dafs.thenParse<SuccessOrErrorResponse>

    let AsyncUnwatch token username =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/unwatch/%s" (Uri.EscapeDataString username))
        |> Dafs.asyncRead
        |> Dafs.thenParse<SuccessOrErrorResponse>

    let AsyncGetWatching token username =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/watching/%s" username)
        |> Dafs.asyncRead
        |> Dafs.thenParse<WatchingResponse>

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
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/profile/%s" (Uri.EscapeDataString req.Username))
        |> Dafs.asyncRead
        |> Dafs.thenParse<Profile>

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
                | ProfileUpdateField.Tagline v -> sprintf "tagline=%s" (Uri.EscapeDataString v)
                | ProfileUpdateField.Countryid v -> sprintf "countryid=%d" v
                | ProfileUpdateField.Website v -> sprintf "website=%s" (Uri.EscapeDataString v)
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/user/profile/update"
        |> Dafs.asyncRead
        |> Dafs.thenParse<SuccessOrErrorResponse>

    let AsyncGetStatus token (id: Guid) =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/statuses/%O" id)
        |> Dafs.asyncRead
        |> Dafs.thenParse<Status>

    let AsyncPageStatuses token (username: string) limit offset =
        seq {
            yield sprintf "username=%s" (Uri.EscapeDataString username)
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/user/statuses"
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<Status>>

    let AsyncGetStatuses token username batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPageStatuses token username batchsize)

    type StatusPostRequest(body: string) =
        member __.Body = body
        member val Statusid = Nullable<Guid>() with get, set
        member val Parentid = Nullable<Guid>() with get, set
        member val Stashid = Nullable<int64>() with get, set

    let AsyncPostStatus token (ps: StatusPostRequest) =
        seq {
            match Option.ofObj ps.Body with
            | Some s -> yield sprintf "body=%s" (Uri.EscapeDataString s)
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
        |> Dafs.thenParse<StatusPostResponse>

    type WatchersRequest() =
        member val Username: string = null with get, set

    let AsyncPageWatchers token expansion (req: FriendsRequest) limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/watchers/%s" (Uri.EscapeDataString req.Username))
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<WatcherRecord>>

    let AsyncGetWatchers token expansion req batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPageWatchers token expansion req batchsize)

    let AsyncWhoami token expansion =
        seq {
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/user/whoami"
        |> Dafs.asyncRead
        |> Dafs.thenParse<User>

    let AsyncWhois token expansion usernames =
        seq {
            for u in usernames do
                yield u |> Uri.EscapeDataString |> sprintf "usernames[]=%s"
                yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/user/whois"
        |> Dafs.asyncRead
        |> Dafs.thenParse<ListOnlyResponse<User>>