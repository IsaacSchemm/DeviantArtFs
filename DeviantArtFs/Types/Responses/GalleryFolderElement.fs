namespace DeviantArtFs

open FSharp.Data

type internal GalleryFoldersElement = JsonProvider<"""[
{
    "folderid": "47D47436-5683-8DF2-EEBF-2A6760BE1336",
    "parent": null,
    "name": "Featured",
    "size": 2
},
{
    "folderid": "E431BAFB-7A00-7EA1-EED7-2EF9FA0F04CE",
    "parent": "47D47436-5683-8DF2-EEBF-2A6760BE1336",
    "name": "My New Gallery"
}
]""", SampleIsList=true>