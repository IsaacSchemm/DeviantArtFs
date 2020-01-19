namespace DeviantArtFs

open FSharp.Json

type DeviantArtCategory = {
    catpath: string
    title: string
    has_subcategory: bool
    parent_catpath: string
}

type DeviantArtCategoryList = {
    categories: DeviantArtCategory list
} with
    static member ParseSeq json =
        let o = Json.deserialize<DeviantArtCategoryList> json
        o.categories :> seq<DeviantArtCategory>