namespace DeviantArtFs.Requests.Feed

open DeviantArtFs

module FeedSettings =
    open FSharp.Control

    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/feed/settings"
        let! json = dafs.asyncRead req
        return DeviantArtFeedSettings.Parse json
    }

    let ExecuteAsync token =
        AsyncExecute token
        |> AsyncThen.map (fun o -> o :> IBclDeviantArtFeedSettings)
        |> Async.StartAsTask