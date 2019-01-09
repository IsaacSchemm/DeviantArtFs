namespace DeviantArtFs

open FSharp.Data

type ProfileResponse = JsonProvider<"""[{
    "user": {
        "userid": "09A4052C-88B2-65CB-CEC5-B1E31C18B940",
        "username": "justgalym",
        "usericon": "https://a.deviantart.net/avatars/j/u/justgalym.jpg?1",
        "type": "regular"
    },
    "is_watching": false,
    "profile_url": "https://justgalym.deviantart.com",
    "user_is_artist": false,
    "artist_level": "Professional",
    "artist_specialty": "Traditional Art",
    "real_name": "Galymzhan",
    "tagline": "my tagline",
    "countryid": 110,
    "country": "Kazakhstan",
    "website": "http://bytespree.com",
    "bio": "my bio",
    "cover_photo": "???",
    "profile_pic": {},
    "last_status": {},
    "stats": {
        "user_deviations": 5,
        "user_favourites": 2,
        "user_comments": 2,
        "profile_pageviews": 3096,
        "profile_comments": 3
    },
    "collections": [
        {
            "folderid": "47B028B2-8D67-C927-3F4F-650E4FABFD6C",
            "name": "Featured"
        },
        {
            "folderid": "7F894824-5609-2C6B-2838-F5E704A27222",
            "name": "Nature"
        },
        {
            "folderid": "38B62271-19EF-403B-0DC4-AE138695B9B6",
            "name": "My Awesome collection"
        }
    ],
    "galleries": [
        {
            "folderid": "C8B0B604-0D86-2076-6207-7D254E175DEA",
            "parent": null,
            "name": "Featured"
        },
        {
            "folderid": "22222222-2222-2222-2222-222222222222",
            "parent": "33333333-3333-3333-3333-333333333333",
            "name": "Example"
        }
    ]
}, {
    "user": {
        "userid": "09A4052C-88B2-65CB-CEC5-B1E31C18B940",
        "username": "justgalym",
        "usericon": "https://a.deviantart.net/avatars/j/u/justgalym.jpg?1",
        "type": "regular"
    },
    "is_watching": false,
    "profile_url": "https://justgalym.deviantart.com",
    "user_is_artist": false,
    "artist_level": "Professional",
    "artist_specialty": null,
    "real_name": "Galymzhan",
    "tagline": "my tagline",
    "countryid": 110,
    "country": "Kazakhstan",
    "website": "http://bytespree.com",
    "bio": "my bio",
    "cover_photo": null,
    "profile_pic": null,
    "last_status": null,
    "stats": {
        "user_deviations": 5,
        "user_favourites": 2,
        "user_comments": 2,
        "profile_pageviews": 3096,
        "profile_comments": 3
    }
}]""", SampleIsList=true>