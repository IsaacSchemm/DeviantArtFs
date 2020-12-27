namespace DeviantArtFs

type DeviantArtCategory = {
    catpath: string
    title: string
    has_subcategory: bool
    parent_catpath: string
}

type DeviantArtCategoryList = {
    categories: DeviantArtCategory list
}