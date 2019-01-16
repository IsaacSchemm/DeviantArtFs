namespace DeviantArtFs.Requests.Data

open DeviantArtFs

type internal CountriesElement = {
    countryid: int
    name: string
}

module Countries =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/data/countries"
        let! json = dafs.asyncRead req
        return json
            |> dafs.parseListOnly (fun _ -> { countryid = 0; name = "" })
            |> Seq.map (fun r -> (r.countryid, r.name))
            |> dict
    }

    let ExecuteAsync token = AsyncExecute token |> Async.StartAsTask