namespace DeviantArtFs

open System

type DeviantArtRenamedNotesFolder = {
    title: string
}

type DeviantArtNewNotesFolder = {
    folder: Guid
    title: string
}

type DeviantArtNotesFolder = {
    folder: Guid
    parentid: Guid option
    title: string
    count: string
}