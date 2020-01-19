namespace DeviantArtFs

open System
open FSharp.Json

type DeviantArtRenamedNotesFolder = {
    title: string
} with
    static member Parse json = Json.deserialize<DeviantArtRenamedNotesFolder> json

type DeviantArtNewNotesFolder = {
    folder: Guid
    title: string
} with
    static member Parse json = Json.deserialize<DeviantArtNewNotesFolder> json

type DeviantArtNotesFolder = {
    folder: Guid
    parentid: Guid option
    title: string
    count: string
} with
    member this.ParentIdOrNull = this.parentid |> Option.toNullable