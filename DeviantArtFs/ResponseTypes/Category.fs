namespace DeviantArtFs.ResponseTypes

type Category = {
    catpath: string
    title: string
    has_subcategory: bool
    parent_catpath: string
}

type CategoryList = {
    categories: Category list
}