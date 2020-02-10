﻿namespace DeviantArtFs

open System
open FSharp.Json

/// A single page of results from a DeviantArt API endpoint with bidirectional
/// paging.
type DeviantArtPagedResult<'a> = {
    has_more: bool
    next_offset: int option
    results: 'a list
} with
    static member Parse json = Json.deserialize<DeviantArtPagedResult<'a>> json
    member this.GetNextOffset() = OptUtils.intDefault this.next_offset
    interface IResultPage<int, 'a> with
        member this.HasMore = this.has_more
        member this.Cursor = this.next_offset |> Option.defaultValue 0
        member this.Items = this.results |> Seq.ofList