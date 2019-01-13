namespace DeviantArtFs

open FSharp.Data

type internal StashMoveResponse = JsonProvider<"""{
    "target": {},
    "changes": []
}""">