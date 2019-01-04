namespace DeviantArtFs

open System
open System.Net
open FSharp.Data

type internal DeviantArtBaseResponse = JsonProvider<"""{"status":"error"}""">

type internal DeviantArtErrorResponse = JsonProvider<"""{"error":"invalid_request","error_description":"Must provide an access_token to access this resource.","status":"error"}""">

type internal SuccessOrErrorResponse = JsonProvider<"""[
{ "success": false, "error_description": "str" },
{ "success": true }
]""", SampleIsList=true>

type internal GenericListResponse = JsonProvider<"""[
{ "has_more": true, "next_offset": 2, "estimated_total": 7, "name": "str", "results": [] },
{ "has_more": false, "next_offset": null, "results": [] }
]""", SampleIsList=true>

type internal ListOnlyResponse = JsonProvider<"""{ "results": [] }""">