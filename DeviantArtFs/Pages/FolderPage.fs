namespace DeviantArtFs.Pages

open DeviantArtFs.ResponseTypes

type FolderPage = {
    has_more: bool
    next_offset: int option
    name: string option
    results: Deviation list
}