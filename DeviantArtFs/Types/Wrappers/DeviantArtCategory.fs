namespace DeviantArtFs

open FSharp.Json

type IBclDeviantArtCategory =
    abstract member Catpath: string
    abstract member Title: string
    abstract member HasSubcategory: bool
    abstract member ParentCatpath: string

type DeviantArtCategory = {
    catpath: string
    title: string
    has_subcategory: bool
    parent_catpath: string
} with
    interface IBclDeviantArtCategory with
        member this.Catpath = this.catpath
        member this.Title = this.title
        member this.HasSubcategory = this.has_subcategory
        member this.ParentCatpath = this.parent_catpath

type internal DeviantArtCategoryList = {
    categories: DeviantArtCategory[]
} with
    static member ParseSeq json =
        let o = Json.deserialize<DeviantArtCategoryList> json
        o.categories :> seq<DeviantArtCategory>
    interface seq<DeviantArtCategory> with
        member this.GetEnumerator() = (this.categories :> seq<DeviantArtCategory>).GetEnumerator()
        member this.GetEnumerator() = this.categories.GetEnumerator()