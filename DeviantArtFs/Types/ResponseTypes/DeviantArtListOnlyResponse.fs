namespace DeviantArtFs

open FSharp.Json

type internal DeviantArtListOnlyResponse<'a> = {
    results: 'a[]
} with
    static member ParseSeq json =
        let o = Json.deserialize<DeviantArtListOnlyResponse<'a>> json
        o.results :> seq<'a>