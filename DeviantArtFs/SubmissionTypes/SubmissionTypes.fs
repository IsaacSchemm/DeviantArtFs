namespace DeviantArtFs.SubmissionTypes

open DeviantArtFs.ParameterTypes

type IFormFile =
    abstract member Filename: string
    abstract member ContentType: string
    abstract member Data: byte[]

module FormFile =
    let Create filename content_type data = {
        new IFormFile with
            member __.Filename = filename
            member __.ContentType = content_type
            member __.Data = data
    }
    
type SubmissionTitle = SubmissionTitle of string | DefaultSubmissionTitle
with static member Default = DefaultSubmissionTitle

type ArtistComments = ArtistComments of string | NoArtistComments
with static member Default = NoArtistComments

type OriginalUrl = OriginalUrl of string | NoOriginalUrl
with static member Default = NoOriginalUrl

type TagList = TagList of string list
with
    static member Create x = TagList (List.ofSeq x)
    static member Empty = TagList []
    
type SubmissionParameters = {
    title: SubmissionTitle
    artist_comments: ArtistComments
    tags: TagList
    original_url: OriginalUrl
    is_dirty: bool
} with
    static member CreateSet x = Set.ofSeq x
    static member Default = {
        title = DefaultSubmissionTitle
        artist_comments = NoArtistComments
        tags = TagList.Empty
        original_url = NoOriginalUrl
        is_dirty = false
    }
    
type SubmissionDestination =
| ReplaceExisting of StashItem
| SubmitToStack of StashStack
| SubmitToStackWithName of string
with static member Default = SubmitToStack RootStack