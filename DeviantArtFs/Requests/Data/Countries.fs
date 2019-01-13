namespace DeviantArtFs.Requests.Data

open DeviantArtFs
open FSharp.Data

type internal CountriesElement = JsonProvider<"""{
    "countryid": 1,
    "name": "United States"
}""">

module Countries =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/data/countries"
        let! json = dafs.asyncRead req
        return json
            |> dafs.parseListOnly CountriesElement.Parse
            |> Seq.map (fun r -> (r.Countryid, r.Name))
            |> dict
    }

    let ExecuteAsync token = AsyncExecute token |> Async.StartAsTask