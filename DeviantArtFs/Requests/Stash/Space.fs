namespace DeviantArtFs.Requests.Stash

open DeviantArtFs
open FSharp.Data

type internal SpaceResponse = JsonProvider<"""{
    "available_space": 10736915095,
    "total_space": 10737418240
}""">

module Space =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/stash/space"

        let! json = dafs.asyncRead req
        let resp = SpaceResponse.Parse json
        return {
            new IStashSpaceResult with
                member __.AvailableSpace = resp.AvailableSpace
                member __.TotalSpace = resp.TotalSpace
        }
    }

    let ExecuteAsync token = AsyncExecute token |> Async.StartAsTask