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

    let AsyncPageFriends token expansion user limit offset =
        let username = match user with | ForUser u -> u | ForCurrentUser -> ""

        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/%s" (Uri.EscapeDataString username))
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<FriendRecord>>

    let AsyncGetFriends token expansion req batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPageFriends token expansion req batchsize)

    let AsyncSearchFriends token user additionalUser q =
        seq {
            yield! QueryFor.userScope user
            yield! QueryFor.additionalUser additionalUser
            yield sprintf "q=%s" (Uri.EscapeDataString q)
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/user/friends/search"
        |> Dafs.asyncRead
        |> Dafs.thenParse<ListOnlyResponse<User>>

    let AsyncWatch token watchTypes username =
        seq {
            yield! QueryFor.watchTypes watchTypes
        }
        |> Dafs.createRequest Dafs.Method.POST token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/friends/watch/%s" (Uri.EscapeDataString username))
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

    let AsyncGetProfile token expansion profile_ext_params user =
        let username = match user with | ForUser u -> u | ForCurrentUser -> ""

        seq {
            yield! QueryFor.profileExtParams profile_ext_params
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/profile/%s" (Uri.EscapeDataString username))
        |> Dafs.asyncRead
        |> Dafs.thenParse<Profile>

    let AsyncUpdateProfile token modifications =
        seq {
            yield! QueryFor.profileModifications modifications
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/user/profile/update"
        |> Dafs.asyncRead
        |> Dafs.thenParse<SuccessOrErrorResponse>

    let AsyncGetStatus token (id: Guid) =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/statuses/%O" id)
        |> Dafs.asyncRead
        |> Dafs.thenParse<Status>

    let AsyncPageStatuses token username limit offset =
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

    let AsyncPostStatus token embeddable_content body =
        seq {
            match Option.ofObj body with
            | Some s -> yield sprintf "body=%s" (Uri.EscapeDataString s)
            | None -> ()
            yield! QueryFor.embeddableStatusContent embeddable_content
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/user/statuses/post"
        |> Dafs.asyncRead
        |> Dafs.thenParse<StatusPostResponse>

    let AsyncPageWatchers token expansion user limit offset =
        let username = match user with | ForUser u -> u | ForCurrentUser -> ""

        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/user/watchers/%s" (Uri.EscapeDataString username))
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