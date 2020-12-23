﻿namespace DeviantArtFs.Api.Stash

open DeviantArtFs

module Space =
    let AsyncExecute token = async {
        let req = Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/stash/space"

        let! json = Dafs.asyncRead req
        return StashSpaceResult.Parse json
    }

    let ExecuteAsync token = AsyncExecute token |> Async.StartAsTask