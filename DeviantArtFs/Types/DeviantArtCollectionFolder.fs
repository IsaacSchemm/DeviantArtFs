namespace DeviantArtFs

open System

type IBclDeviantArtCollectionFolder =
    abstract member Folderid: Guid
    abstract member Name: string
    abstract member Size: Nullable<int>

type DeviantArtCollectionFolder(folder: CollectionFoldersElement.Root) =
    member __.Folderid = folder.Folderid
    member __.Name = folder.Name
    member __.Size = folder.Size
    interface IBclDeviantArtCollectionFolder with
        member this.Folderid = this.Folderid
        member this.Name = this.Name
        member this.Size = this.Size |> Option.toNullable