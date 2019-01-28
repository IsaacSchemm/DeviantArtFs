namespace DeviantArtFs

open System

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