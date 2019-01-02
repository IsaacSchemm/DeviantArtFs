namespace DeviantArtFs

open FSharp.Data

type DeviationResponse = JsonProvider<"""[{
    "deviationid": "F9921FC4-3A0F-90A9-3625-AA8E105747AD",
    "printid": null,
    "url": "https://justgalym.deviantart.com/journal/Another-post-written-in-stash-446384730",
    "title": "Another post written in stash",
    "category": "Personal Journal",
    "category_path": "personaljournal",
    "is_favourited": false,
    "is_deleted": false,
    "author": {
        "userid": "09A4052C-88B2-65CB-CEC5-B1E31C18B940",
        "username": "justgalym",
        "usericon": "https://a.deviantart.net/avatars/j/u/justgalym.jpg?1",
        "type": "admin"
    },
    "stats": {
        "comments": 0,
        "favourites": 0
    },
    "published_time": 2222222222,
    "allows_comments": true,
    "excerpt": "I'm checking out stash writer",
    "thumbs": []
}, {
    "deviationid": "1509BE0C-94F0-C29E-002A-5BE0A3192208",
    "printid": null,
    "url": "https://www.deviantart.com/minnoux/art/c-KatNikki-778616560",
    "title": "c: KatNikki",
    "category": "Drawings",
    "category_path": "manga/digital/drawings",
    "is_favourited": false,
    "is_deleted": false,
    "author": {
        "userid": "72AC83A6-67D4-9834-713E-38B67D9133F6",
        "username": "minnoux",
        "usericon": "https://a.deviantart.net/avatars/m/i/minnoux.png?4",
        "type": "regular"
    },
    "stats": {
        "comments": 15,
        "favourites": 537
    },
    "published_time": "1546025319",
    "allows_comments": true,
    "preview": {
        "src": "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/intermediary/f/9d7589dd-d8c8-4608-98db-44bac2540fbd/dcvkgds-ad671466-e7c5-4ae0-935b-24e1cfd9cf96.png/v1/fill/w_800,h_846,q_80,strp/c__katnikki_by_minnoux_dcvkgds-fullview.jpg",
        "height": 846,
        "width": 800,
        "transparency": false
    },
    "content": {
        "src": "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/intermediary/f/9d7589dd-d8c8-4608-98db-44bac2540fbd/dcvkgds-ad671466-e7c5-4ae0-935b-24e1cfd9cf96.png/v1/fill/w_800,h_846,q_80,strp/c__katnikki_by_minnoux_dcvkgds-fullview.jpg",
        "height": 846,
        "width": 800,
        "transparency": false,
        "filesize": 1707821
    },
    "thumbs": [
        {
            "src": "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/intermediary/f/9d7589dd-d8c8-4608-98db-44bac2540fbd/dcvkgds-ad671466-e7c5-4ae0-935b-24e1cfd9cf96.png/v1/fit/w_150,h_150,q_70,strp/c__katnikki_by_minnoux_dcvkgds-150.jpg",
            "height": 150,
            "width": 142,
            "transparency": false
        },
        {
            "src": "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/intermediary/f/9d7589dd-d8c8-4608-98db-44bac2540fbd/dcvkgds-ad671466-e7c5-4ae0-935b-24e1cfd9cf96.png/v1/fill/w_189,h_200,q_70,strp/c__katnikki_by_minnoux_dcvkgds-200h.jpg",
            "height": 200,
            "width": 189,
            "transparency": false
        },
        {
            "src": "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/intermediary/f/9d7589dd-d8c8-4608-98db-44bac2540fbd/dcvkgds-ad671466-e7c5-4ae0-935b-24e1cfd9cf96.png/v1/fit/w_300,h_900,q_70,strp/c__katnikki_by_minnoux_dcvkgds-300w.jpg",
            "height": 317,
            "width": 300,
            "transparency": false
        }
    ],
    "daily_deviation": {
        "body": "<a href=\"https://www.deviantart.com/deviation/778616560/\">c: KatNikki</a> by <span class=\"username-with-symbol u\"><a class=\"u regular username\" href=\"https://www.deviantart.com/minnoux\">minnoux</a><span class=\"user-symbol regular\" data-quicktip-text=\"\" data-show-tooltip=\"\" data-gruser-type=\"regular\"></span></span>",
        "time": "2019-01-02T00:00:00-0800",
        "giver": {
            "userid": "A9599FCB-46AA-FCE7-7121-B9171F1F57B8",
            "username": "AngeKrystaleen",
            "usericon": "https://a.deviantart.net/avatars/a/n/angekrystaleen.gif?11",
            "type": "volunteer"
        },
        "suggester": {
            "userid": "65EE8992-F0B0-9D74-CC08-8DFE47C74DF6",
            "username": "Catgirldstr11",
            "usericon": "https://a.deviantart.net/avatars/c/a/catgirldstr11.png?9",
            "type": "beta"
        }
    },
    "is_mature": false,
    "is_downloadable": false
}, {
    "deviationid": "00000000-0000-0000-0000-000000000000",
    "printid": "00000000-0000-0000-0000-000000000000",
    "is_deleted": true
}]""", SampleIsList=true>