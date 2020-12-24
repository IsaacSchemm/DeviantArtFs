namespace DeviantArtFs.Api.Data

open DeviantArtFs

module Privacy =
    let AsyncExecute token =
        Seq.empty
        |> Dafs.createRequest2 token "https://www.deviantart.com/api/v1/oauth2/data/privacy"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtTextOnlyResponse>
        |> Dafs.extractText

    let ExecuteAsync token =
        AsyncExecute token
        |> Async.StartAsTask