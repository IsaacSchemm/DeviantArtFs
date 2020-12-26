namespace DeviantArtFs.Api.Data

open DeviantArtFs

type CountriesElement = {
    countryid: int
    name: string
}

module Countries =
    let AsyncExecute token =
        Seq.empty
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/data/countries"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtListOnlyResponse<CountriesElement>>
        |> Dafs.extractList

    let ExecuteAsync token =
        AsyncExecute token
        |> Async.StartAsTask