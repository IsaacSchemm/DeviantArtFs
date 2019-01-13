namespace DeviantArtFs

open FSharp.Data

type internal CollectionFoldersElement = JsonProvider<"""[
{
    "folderid": "79A8B981-131F-C7B6-C5C0-2CDA5D5B8B29",
    "name": "Featured",
    "size": 1535
},
{
    "folderid": "FF02CACB-466D-E167-A841-386078AAD276",
    "name": "Favorites"
}
]""", SampleIsList=true>