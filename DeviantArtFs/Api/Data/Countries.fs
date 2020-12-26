namespace DeviantArtFs.Api.Data

open DeviantArtFs

type CountriesElement = {
    countryid: int
    name: string
}

module Countries =
    let AsyncExecute token common =
        seq {
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/data/countries"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtListOnlyResponse<CountriesElement>>

    let ExecuteAsync token common =
        AsyncExecute token common
        |> Async.StartAsTask