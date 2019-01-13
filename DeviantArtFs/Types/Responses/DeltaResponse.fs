namespace DeviantArtFs

open FSharp.Data

type internal DeltaResponse = JsonProvider<"""[{
    "cursor": "cursor1",
    "has_more": true,
    "next_offset": 42,
    "reset": false,
    "entries": [
        {
            "stackid": 1555555555555555,
            "metadata": {},
            "position": 40
        },
        {
            "itemid": 5555555555555555,
            "stackid": 88888888888880,
            "metadata": {},
            "position": 0
        },
        {
            "itemid": 7777777777777777,
            "stackid": null,
            "metadata": null
        }
    ]
}, {
    "cursor": "cursor2",
    "has_more": false,
    "next_offset": null,
    "reset": false,
    "entries": []
}]""", SampleIsList=true>