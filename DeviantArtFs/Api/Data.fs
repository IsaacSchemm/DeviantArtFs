namespace DeviantArtFs.Api

open DeviantArtFs
open DeviantArtFs.ResponseTypes

module Data =
    let AsyncGetCountries token =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/data/countries"
        |> Dafs.asyncRead
        |> Dafs.thenParse<ListOnlyResponse<Country>>

    let AsyncGetPrivacyPolicy token =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/data/privacy"
        |> Dafs.asyncRead
        |> Dafs.thenParse<TextOnlyResponse>

    let AsyncGetSubmissionPolicy token =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/data/submission"
        |> Dafs.asyncRead
        |> Dafs.thenParse<TextOnlyResponse>

    let AsyncGetTermsOfService token =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/data/tos"
        |> Dafs.asyncRead
        |> Dafs.thenParse<TextOnlyResponse>