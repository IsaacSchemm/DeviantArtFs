namespace DeviantArtFs

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
}

type DeviantArtCategoryList = {
    categories: DeviantArtCategory list
}