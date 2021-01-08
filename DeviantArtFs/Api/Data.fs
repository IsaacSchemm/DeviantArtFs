namespace DeivantArtFs.Api

open DeviantArtFs

module Data =
    let AsyncGetCountries token =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/data/countries"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtListOnlyResponse<DeviantArtCountry>>

    let AsyncGetPrivacyPolicy token =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/data/privacy"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtTextOnlyResponse>

    let AsyncGetSubmissionPolicy token =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/data/submission"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtTextOnlyResponse>

    let AsyncGetTermsOfService token =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/data/tos"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtTextOnlyResponse>