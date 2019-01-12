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
    "collections": [],
    "galleries": []
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