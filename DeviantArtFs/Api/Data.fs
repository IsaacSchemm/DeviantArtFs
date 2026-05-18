namespace DeviantArtFs.Api

open DeviantArtFs
open DeviantArtFs.ResponseTypes

module Data =
    type Country = {
       countryid: int
       name: string
    }

    let AsyncGetCountries token =
        Seq.empty
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/data/countries"
        |> Utils.asyncRead
        |> Utils.thenParse<ListOnlyResponse<Country>>

    let AsyncGetPrivacyPolicy token =
        Seq.empty
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/data/privacy"
        |> Utils.asyncRead
        |> Utils.thenParse<TextOnlyResponse>

    let AsyncGetSubmissionPolicy token =
        Seq.empty
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/data/submission"
        |> Utils.asyncRead
        |> Utils.thenParse<TextOnlyResponse>

    let AsyncGetTermsOfService token =
        Seq.empty
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/data/tos"
        |> Utils.asyncRead
        |> Utils.thenParse<TextOnlyResponse>
