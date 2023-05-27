namespace DeviantArtFs.Api

open DeviantArtFs
open DeviantArtFs.ResponseTypes

module Data =
    type Country = {
       countryid: int
       name: string
    }

    let GetCountriesAsync token =
        Seq.empty
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/data/countries"
        |> Utils.readAsync
        |> Utils.thenParse<ListOnlyResponse<Country>>

    let GetPrivacyPolicyAsync token =
        Seq.empty
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/data/privacy"
        |> Utils.readAsync
        |> Utils.thenParse<TextOnlyResponse>

    let GetSubmissionPolicyAsync token =
        Seq.empty
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/data/submission"
        |> Utils.readAsync
        |> Utils.thenParse<TextOnlyResponse>

    let GetTermsOfServiceAsync token =
        Seq.empty
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/data/tos"
        |> Utils.readAsync
        |> Utils.thenParse<TextOnlyResponse>