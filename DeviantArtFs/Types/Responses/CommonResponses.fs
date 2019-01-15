namespace DeviantArtFs

open FSharp.Data

type internal GenericListResponse = JsonProvider<"""[
{
    "has_more": true,
    "next_offset": 2,
    "estimated_total": 4,
    "has_less": true,
    "prev_offset": 1,
    "name": "str",
    "results": []
},
{
    "has_more": false,
    "next_offset": null,
    "results": []
}
]""", SampleIsList=true>

type internal ListOnlyResponse = JsonProvider<"""{ "results": [] }""">