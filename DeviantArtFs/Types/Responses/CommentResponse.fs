namespace DeviantArtFs

open FSharp.Data

type internal CommentResponse = JsonProvider<"""[
{
    "commentid": "00000000-0000-0000-0000-000000000000",
    "parentid": "11111111-1111-1111-1111-111111111111",
    "posted": "2018-12-25T02:24:54-0800",
    "replies": 0,
    "hidden": null,
    "body": "well d a m n",
    "user": {
        "userid": "FE89BDF5-90DF-951A-24CD-366353ECC271",
        "username": "libertyernie",
        "usericon": "https://a.deviantart.net/avatars/l/i/libertyernie.jpg?2",
        "type": "regular"
    }
},
{
    "commentid": "00000000-0000-0000-0000-000000000000",
    "parentid": null,
    "posted": "2018-12-24T21:15:09-0800",
    "replies": 0,
    "hidden": "hidden_by_owner",
    "body": "[Hidden by Owner]",
    "user": {
        "userid": "FE89BDF5-90DF-951A-24CD-366353ECC271",
        "username": "libertyernie",
        "usericon": "https://a.deviantart.net/avatars/l/i/libertyernie.jpg?2",
        "type": "regular"
    }
},
{
    "commentid": "00000000-0000-0000-0000-000000000000",
    "parentid": null,
    "posted": "2018-12-24T18:22:07-0800",
    "replies": 2,
    "hidden": null,
    "body": "Cute!",
    "user": {
        "userid": "FE89BDF5-90DF-951A-24CD-366353ECC271",
        "username": "libertyernie",
        "usericon": "https://a.deviantart.net/avatars/l/i/libertyernie.jpg?2",
        "type": "regular"
    }
}
]""", SampleIsList=true>