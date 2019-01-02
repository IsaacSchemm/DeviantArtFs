namespace DeviantArtFs.Requests.User

open DeviantArtFs
open FSharp.Data

type WhoamiRepsonse = JsonProvider<"""{
    "userid": "CAFD9087-C6EF-2F2C-183B-A658AE61FB95",
    "username": "verycoolusername",
    "usericon": "https://a.deviantart.net/avatars/default.gif",
    "type": "regular"
}""">

module Whoami =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/user/whoami"
        let! json = dafs.asyncRead req
        let u = WhoamiRepsonse.Parse json
        return {
            Userid = u.Userid
            Username = u.Username
            Usericon = u.Usericon
            Type = u.Type
        }
    }

    let ExecuteAsync token = AsyncExecute token |> Async.StartAsTask