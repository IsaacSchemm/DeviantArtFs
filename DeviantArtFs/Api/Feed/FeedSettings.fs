namespace DeviantArtFs.Api.Feed

open DeviantArtFs

module FeedSettings =
    open FSharp.Control

    let AsyncExecute token = async {
        let req = Dafs.createRequest token DeviantArtCommonParams.Default "https://www.deviantart.com/api/v1/oauth2/feed/settings"
        let! json = Dafs.asyncRead req
        return DeviantArtFeedSettings.Parse json
    }

    let ExecuteAsync token =
        AsyncExecute token
        |> Async.StartAsTask