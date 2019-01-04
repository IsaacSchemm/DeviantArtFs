namespace DeviantArtFs

open System

[<AllowNullLiteral>]
type IDeviantArtUser =
    abstract member Userid: Guid
    abstract member Username: string
    abstract member Usericon: string
    abstract member Type: string

type DeviantArtUser = {
    Userid: Guid
    Username: string
    Usericon: string
    Type: string
} with
    interface IDeviantArtUser with
        member this.Userid = this.Userid
        member this.Username = this.Username
        member this.Usericon = this.Usericon
        member this.Type = this.Type