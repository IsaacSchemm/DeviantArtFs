namespace DeviantArtFs

open FSharp.Json

type DeviantArtSendNoteResult = {
    success: bool
    user: DeviantArtUser
}