namespace DeviantArtFs.Api.Data

open DeviantArtFs

type CountriesElement = {
    countryid: int
    name: string
}

module Countries =
    let AsyncExecute token = async {
        let req = Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/data/countries"
        let! json = Dafs.asyncRead req
        return json
            |> DeviantArtListOnlyResponse.ParseList
            |> Seq.map (fun r -> (r.countryid, r.name))
            |> dict
    }

    let ExecuteAsync token = AsyncExecute token |> Async.StartAsTask