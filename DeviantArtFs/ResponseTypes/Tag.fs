namespace DeviantArtFs.ResponseTypes

type Tag = {
    tag_name: string
    sponsored: bool
    sponsor: string option
}