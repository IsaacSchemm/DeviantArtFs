﻿namespace DeviantArtFs

open System
open FSharp.Json

type DeviantArtUser = {
    userid: Guid
    username: string
    usericon: string
    ``type``: string
} with
    static member Parse json = Json.deserialize<DeviantArtUser> json
    interface IDeviantArtUser with
        member this.Userid = this.userid
        member this.Username = this.username
        member this.Usericon = this.usericon
        member this.Type = this.``type``