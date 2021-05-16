namespace DeviantArtFs.ResponseTypes

open System

type StashPublishResponse = {
    status: string
    url: string
    deviationid: Guid
}