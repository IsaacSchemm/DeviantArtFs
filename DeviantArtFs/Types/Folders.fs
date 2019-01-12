namespace DeviantArtFs

open System

type IDeviantArtCollection =
    abstract member Folderid: Guid
    abstract member Name: string