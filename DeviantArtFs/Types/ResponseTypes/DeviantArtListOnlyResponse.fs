namespace DeviantArtFs

open System
open FSharp.Json

type DeviantArtListOnlyResponse<'a> = {
    results: 'a[]
} with
    static member Parse json = Json.deserialize<DeviantArtListOnlyResponse<'a>> json
    interface seq<'a> with
        member this.GetEnumerator() = this.results.GetEnumerator()
        member this.GetEnumerator() = (this.results :> seq<'a>).GetEnumerator()