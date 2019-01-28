namespace DeviantArtFs

open System
open FSharp.Json

//type IBclDeviantArtRenamedNotesFolder =
//    abstract member Title: string

//type DeviantArtRenamedNotesFolder = {
//    title: string
//} with
//    static member Parse json = Json.deserialize<DeviantArtRenamedNotesFolder> json
//    interface IBclDeviantArtRenamedNotesFolder with
//        member this.Title = this.title

type IBclDeviantArtNewNotesFolder =
    abstract member Folder: Guid
    abstract member Title: string

type DeviantArtNewNotesFolder = {
    folder: Guid
    title: string
} with
    static member Parse json = Json.deserialize<DeviantArtNewNotesFolder> json
    interface IBclDeviantArtNewNotesFolder with
        member this.Folder = this.folder
        member this.Title = this.title

type IBclDeviantArtNotesFolder =
    abstract member Folder: Guid
    abstract member Parentid: Nullable<Guid>
    abstract member Title: string
    abstract member Count: string

type DeviantArtNotesFolder = {
    folder: Guid
    parentid: Guid option
    title: string
    count: string
} with
    interface IBclDeviantArtNotesFolder with
        member this.Count = this.count
        member this.Folder = this.folder
        member this.Parentid = this.parentid |> Option.toNullable
        member this.Title = this.title