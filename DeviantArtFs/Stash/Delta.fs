namespace DeviantArtFs.Stash

open DeviantArtFs
open FSharp.Data

type DeltaResponse = JsonProvider<"""[{
    "cursor": "678ce5dfc5ebcd029db5b0f5720fb8fc",
    "has_more": true,
    "next_offset": 42,
    "reset": false,
    "entries": [
        {
            "stackid": 1555555555555555,
            "metadata": {
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
            "position": 40
        },
        {
            "itemid": 5555555555555555,
            "stackid": 88888888888880,
            "metadata": {
                "title": "Title 2",
                "description": null,
                "artist_comments": "<b>comments</b>",
                "original_url": "https://wwww.example.com",
                "category": "anthro/digital/other",
                "creation_time": 1539568626,
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
            "position": 0
        },
        {
            "itemid": 7777777777777777,
            "stackid": 3555555555555551,
            "metadata": {
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
            "position": 0
        }
    ]
}, {
    "cursor": "366d90716db162067d08775c20f72abe",
    "has_more": false,
    "next_offset": null,
    "reset": false,
    "entries": []
}]""", SampleIsList=true>

type DeltaRequest() = 
    member val Cursor = null with get, set
    member val Offset = 0 with get, set
    member val Limit = 120 with get, set
    member val ExtSubmission = false with get, set
    member val ExtCamera = false with get, set
    member val ExtStats = false with get, set

module Delta =
    let AsyncExecute token (req: DeltaRequest) = async {
        let query = seq {
            match Option.ofObj req.Cursor with
            | Some s -> yield sprintf "cursor=%s" (dafs.urlEncode s)
            | None -> ()
            yield sprintf "offset=%d" req.Offset
            yield sprintf "limit=%d" req.Limit
            yield sprintf "ext_submission=%b" req.ExtSubmission
            yield sprintf "ext_camera=%b" req.ExtCamera
            yield sprintf "ext_stats=%b" req.ExtStats
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/stash/delta?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return DeltaResponse.Parse json
    }