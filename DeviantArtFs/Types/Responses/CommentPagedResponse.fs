namespace DeviantArtFs

open FSharp.Data

type internal CommentPagedResponse = JsonProvider<"""[
{
    "has_more": true,
    "next_offset": 2,
    "has_less": false,
    "prev_offset": null,
    "total": 3,
    "thread": []
},
{
    "has_more": false,
    "next_offset": null,
    "has_less": true,
    "prev_offset": 0,
    "thread": []
}
]""", SampleIsList=true>