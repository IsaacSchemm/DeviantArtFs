﻿namespace DeviantArtFs.Stash

open DeviantArtFs
open FSharp.Data

type StackResponse = JsonProvider<"""[
{
    "title": "Title 1",
    "path": "Saved Submissions/Sta.sh Uploads 367",
    "size": 1,
    "description": "",
    "parentid": 8888888888888888,
    "thumb": {
        "src": "https://www.example.com",
        "height": 128,
        "width": 128,
        "transparency": false
    },
    "stackid": 1555555555555555
},
{
    "title": "Title 2",
    "description": null,
    "artist_comments": "<b>comments</b>",
    "original_url": "https://wwww.example.com",
    "category": "anthro/digital/other",
    "creation_time": 2208988800,
    "files": [
        {
            "src": "https://www.example.com",
            "height": 760,
            "width": 780,
            "transparency": false
        }
    ],
    "submission": {},
    "stats": {},
    "stackid": 88888888888880,
    "itemid": 5555555555555555,
    "tags": ["tag1"]
},
{
    "title": "100 0500",
    "description": "Something",
    "artist_comments": "",
    "original_url": "",
    "category": "",
    "creation_time": 1532042965,
    "files": [
        {
            "src": "https://www.example.com",
            "height": 1440,
            "width": 2160,
            "transparency": false
        }
    ],
    "submission": {
        "file_size": "358 KB",
        "resolution": "2160x1440",
        "submitted_with": {
            "app": "Sta.sh",
            "url": "https://sta.sh"
        }
    },
    "stats": {
        "views": 0,
        "views_today": 0,
        "downloads": 0,
        "downloads_today": 0
    },
    "camera": {},
    "stackid": 3555555555555551,
    "itemid": 7777777777777777,
    "tags": []
},
{}
]""", SampleIsList=true>

module Stack =
    let AsyncExecute token (stackid: int64) = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/stash/%d" stackid
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return StackResponse.Parse json
    }