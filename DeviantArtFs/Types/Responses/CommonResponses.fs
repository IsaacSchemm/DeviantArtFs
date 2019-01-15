namespace DeviantArtFs

open FSharp.Data

type internal UserResponse = JsonProvider<"""{
    "userid": "CAFD9087-C6EF-2F2C-183B-A658AE61FB95",
    "username": "verycoolusername",
    "usericon": "https://a.deviantart.net/avatars/default.gif",
    "type": "regular"
}""">

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

type internal TextOnlyResponse = JsonProvider<"""{ "text": "html_content" }""">