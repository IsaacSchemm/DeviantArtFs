﻿namespace DeviantArtFs

open System
open FSharp.Json

/// A single page of results from a DeviantArt API endpoint.
type DeviantArtBrowsePagedResult = {
    has_more: bool
    next_offset: int option
    estimated_total: int option
    results: Deviation list
} with
    static member Parse json = Json.deserialize<DeviantArtBrowsePagedResult> json
    member this.GetNextOffset() = OptUtils.intDefault this.next_offset
    member this.GetEstimatedTotal() = OptUtils.intDefault this.estimated_total
    interface IResultPage<int, Deviation> with
        member this.HasMore = this.has_more
        member this.Cursor = this.next_offset |> Option.defaultValue 0
        member this.Items = this.results |> Seq.ofList