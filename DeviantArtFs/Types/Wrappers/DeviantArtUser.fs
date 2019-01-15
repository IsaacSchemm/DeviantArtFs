namespace DeviantArtFs

open System
open FSharp.Json

type DeviantArtUser = {
    Userid: Guid
    Username: string
    Usericon: string
    Type: string
} with
    static member Parse json = Json.deserialize<DeviantArtUser> json
    interface IDeviantArtUser with
        member this.Userid = this.Userid
        member this.Username = this.Username
        member this.Usericon = this.Usericon
        member this.Type = this.Type