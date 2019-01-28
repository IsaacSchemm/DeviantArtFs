namespace DeviantArtFs

open FSharp.Json

type IBclDeviantArtSendNoteResult =
    abstract member Success: bool
    abstract member User: IBclDeviantArtUser

type DeviantArtSendNoteResult = {
    success: bool
    user: DeviantArtUser
} with
    interface IBclDeviantArtSendNoteResult with
        member this.Success = this.success
        member this.User = this.user :> IBclDeviantArtUser