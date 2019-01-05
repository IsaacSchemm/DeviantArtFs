namespace DeviantArtFs.Requests.Data

open DeviantArtFs
open FSharp.Data

type CountriesElement = JsonProvider<"""{
    "countryid": 1,
    "name": "United States"
}""">

module Countries =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/data/countries"
        let! json = dafs.asyncRead req
        return dafs.parseListOnly CountriesElement.Parse json
    }

    let ExecuteAsync token = Async.StartAsTask (async {
        let! obj = AsyncExecute token
        return obj
            |> Seq.map (fun r -> (r.Countryid, r.Name))
            |> dict
    })