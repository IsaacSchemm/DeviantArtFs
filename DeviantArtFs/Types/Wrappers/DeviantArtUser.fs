namespace DeviantArtFs

open System
open FSharp.Json

[<AllowNullLiteral>]
type IBclDeviantArtUser =
    abstract member Userid: Guid
    abstract member Username: string
    abstract member Usericon: string
    abstract member Type: string

type DeviantArtUser = {
    userid: Guid
    username: string
    usericon: string
    ``type``: string
} with
    static member Parse json = Json.deserialize<DeviantArtUser> json
    interface IBclDeviantArtUser with
        member this.Userid = this.userid
        member this.Username = this.username
        member this.Usericon = this.usericon
        member this.Type = this.``type``