namespace DeviantArtFs

open FSharp.Json

type DeviantArtListOnlyResponse<'a> = {
    results: 'a list
} with
    static member ParseSeq json =
        let o = Json.deserialize<DeviantArtListOnlyResponse<'a>> json
        o.results :> seq<'a>