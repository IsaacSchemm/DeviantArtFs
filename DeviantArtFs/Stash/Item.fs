namespace DeviantArtFs.Stash

open DeviantArtFs
open FSharp.Data

type ItemResponse = JsonProvider<"""[
{
    "itemid": 5555555555555555,
    "stackid": 88888888888888,
    "title": "Title",
    "path": "???",
    "description": "<b>html</b>",
    "parentid": 6666666666666666,
    "submission": {},
    "stats": {}
},
{
    "itemid": 4444444444444444,
    "stackid": 7777777777777770,
    "title": "sfgsdagfdgsg",
    "description": null,
    "artist_comments": "",
    "tags": [],
    "original_url": "",
    "category": "",
    "creation_time": 1545507135,
    "html": "<b>made with sta.sh writer</b>"
},
{
    "itemid": 7777777777777777,
    "stackid": 3333333333333333,
    "title": "Title",
    "description": null,
    "artist_comments": "<b>comments</b>",
    "tags": ["tag1", "tag2"],
    "original_url": "https://www.example.com",
    "category": "anthro/digital/other",
    "creation_time": 2222222222,
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
    "camera": {}
}
]""", SampleIsList=true>

type ItemRequest(itemid: int64) = 
    member __.Itemid = itemid
    member val ExtParams = new ExtParams() with get, set

module Item =
    let AsyncExecute token (req: ItemRequest) = async {
        let query = seq {
            yield sprintf "ext_submission=%b" req.ExtParams.ExtSubmission
            yield sprintf "ext_camera=%b" req.ExtParams.ExtCamera
            yield sprintf "ext_stats=%b" req.ExtParams.ExtStats
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/stash/item/%d?%s" req.Itemid
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return ItemResponse.Parse json
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask