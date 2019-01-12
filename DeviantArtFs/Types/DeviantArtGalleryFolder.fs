namespace DeviantArtFs

open System

type IBclDeviantArtGalleryFolder =
    abstract member Folderid: Guid
    abstract member Parent: Nullable<Guid>
    abstract member Name: string
    abstract member Size: Nullable<int>

type DeviantArtGalleryFolder(folder: GalleryFoldersElement.Root) =
    member __.Folderid = folder.Folderid
    member __.Parent = folder.Parent
    member __.Name = folder.Name
    member __.Size = folder.Size
    interface IBclDeviantArtGalleryFolder with
        member this.Folderid = this.Folderid
        member this.Parent = this.Parent |> Option.toNullable
        member this.Name = this.Name
        member this.Size = this.Size |> Option.toNullable