﻿namespace DeviantArtFs

open FSharp.Data

type internal DeviantArtBaseResponse = JsonProvider<"""{"status":"error"}""">

type internal DeviantArtErrorResponse = JsonProvider<"""{"error":"invalid_request","error_description":"Must provide an access_token to access this resource.","status":"error"}""">

type internal SuccessOrErrorResponse = JsonProvider<"""[
{ "success": false, "error_description": "str" },
{ "success": true }
]""", SampleIsList=true>

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