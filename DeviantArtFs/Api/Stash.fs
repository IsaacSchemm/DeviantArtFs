namespace DeviantArtFs.Api

open System
open System.IO
open DeviantArtFs
open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages
open System.Net.Http
open FSharp.Control
open System.Net.Http.Headers

module Stash =
    type Stack = Stack of int64 | RootStack
    type Item = Item of int64

    let private stackid stack = match stack with Stack x -> x | RootStack -> 0L
    let private itemid item = match item with Item x -> x

    type DeltaCursor = DeltaCursor of string | Initial

    type SubmissionDestination =
    | ReplaceExisting of Item
    | SubmitToStack of Stack
    | SubmitToStackWithName of string
    with static member Default = SubmitToStack RootStack

    let GetStackAsync token stack =
        Seq.empty
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/stash/{stackid stack}"
        |> Utils.readAsync
        |> Utils.thenParse<StashMetadata>

    let PageContentsAsync token stack limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/stash/{stackid stack}/contents"
        |> Utils.readAsync
        |> Utils.thenParse<Page<StashMetadata>>

    let GetContentsAsync token stack batchsize offset = taskSeq {
        let mutable offset = offset
        let mutable has_more = true
        while has_more do
            let! data = PageContentsAsync token stack batchsize offset
            yield! data.results.Value
            has_more <- data.has_more.Value
            if has_more then
                offset <- PagingOffset data.next_offset.Value
    }

    let DeleteAsync token item =
        seq {
            "itemid", string (itemid item)
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/stash/delete"
        |> Utils.readAsync
        |> Utils.thenParse<SuccessOrErrorResponse>

    type StashDeltaEntry = {
        itemid: int64 option
        stackid: int64 option
        metadata: StashMetadata option
        position: int option
    }

    type StashDelta = {
        cursor: string
        has_more: bool
        next_offset: int option
        reset: bool
        entries: StashDeltaEntry list
    }

    let PageDeltaAsync token cursor limit offset =
        seq {
            match cursor with
            | DeltaCursor c -> "cursor", c
            | Initial -> ()
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 120
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/stash/delta"
        |> Utils.readAsync
        |> Utils.thenParse<StashDelta>

    let GetDeltaAsync token cursor batchsize offset = taskSeq {
        let mutable offset = offset
        let mutable has_more = true
        while has_more do
            let! data = PageDeltaAsync token cursor batchsize offset
            yield! data.entries
            has_more <- data.has_more
            if has_more then
                offset <- PagingOffset data.next_offset.Value
    }

    let GetItemAsync token item =
        Seq.empty
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/stash/item/{itemid item}"
        |> Utils.readAsync
        |> Utils.thenParse<StashMetadata>

    type StashMoveResult = {
        target: StashMetadata
        changes: StashMetadata list
    }

    let MoveItemAsync token stack (targetid: int64) =
        seq {
            yield "targetid", string targetid
        }
        |> Utils.post token $"https://www.deviantart.com/api/v1/oauth2/stash/move/{stackid stack}"
        |> Utils.readAsync
        |> Utils.thenParse<StashMoveResult>

    let PositionItemAsync token stack (position: int) =
        seq {
            yield "position", string position
        }
        |> Utils.post token $"https://www.deviantart.com/api/v1/oauth2/stash/position/{stackid stack}"
        |> Utils.readAsync
        |> Utils.thenParse<SuccessOrErrorResponse>

    type GallerySet = GallerySet of Guid Set
    with
        static member Create x = GallerySet (Set.ofSeq x)
        static member Empty = GallerySet Set.empty

    type DisplayResolution =
    | Original=0
    | Max400Px=1
    | Max600px=2
    | Max800px=3
    | Max900px=4
    | Max1024px=5
    | Max1280px=6
    | Max1600px=7
    | Max1920px=8

    type Sharing = AllowSharing | HideShareButtons | HideShareButtonsAndMembersOnly

    type PublishParameter =
    | Maturity of Maturity
    | SubmissionPolicyAgreement of bool
    | TermsOfServiceAgreement of bool
    | Featured of bool
    | AllowComments of bool
    | RequestCritique of bool
    | DisplayResolution of DisplayResolution
    | Sharing of Sharing
    | License of License
    | GalleryId of Guid
    | AllowFreeDownload of bool
    | AddWatermark of bool

    type StashPublishResponse = {
        status: string
        url: string
        deviationid: Guid
    }

    let PublishAsync token publishParameters item =
        seq {
            for p in publishParameters do
                match p with
                | Maturity NotMature ->
                    "is_mature", "0"
                | Maturity (Mature (level, classifications)) ->
                    "is_mature", "1"
                    match level with
                    | MatureStrict -> "mature_level", "strict"
                    | MatureModerate -> "mature_level", "moderate"
                    for classification in classifications do
                        match classification with
                        | Nudity -> "mature_classification[]", "nudity"
                        | Sexual -> "mature_classification[]", "sexual"
                        | Gore -> "mature_classification[]", "gore"
                        | Language -> "mature_classification[]", "language"
                        | Ideology -> "mature_classification[]", "ideology"
                | SubmissionPolicyAgreement true -> "agree_submission", "1"
                | SubmissionPolicyAgreement false -> "agree_submission", "0"
                | TermsOfServiceAgreement true -> "agree_tos", "1"
                | TermsOfServiceAgreement false -> "agree_tos", "0"
                | Featured true -> "feature", "1"
                | Featured false -> "feature", "0"
                | AllowComments true -> "allow_comments", "1"
                | AllowComments false -> "allow_comments", "0"
                | RequestCritique true -> "request_critique", "1"
                | RequestCritique false -> "request_critique", "0"
                | DisplayResolution d -> "display_resolution", string (int d)
                | Sharing AllowSharing -> "sharing", "allow"
                | Sharing HideShareButtons -> "sharing", "hide_share_buttons"
                | Sharing HideShareButtonsAndMembersOnly -> "sharing", "hide_and_members_only"
                | License DefaultLicense ->
                    "license_options[creative_commons]", "0"
                | License (CreativeCommonsLicense license) ->
                    "license_options[creative_commons]", "1"
                    match license.commercialUse with
                    | CommercialUsePermitted -> "license_options[commercial]", "yes"
                    | NonCommercial -> "license_options[commercial]", "no"
                    match license.derivativeWorks with
                    | DerivativeWorksPermitted -> "license_options[modify]", "yes"
                    | NoDerivatives -> "license_options[modify]", "no"
                    | ShareAlike -> "license_options[modify]", "share"
                | GalleryId g ->
                    "galleryids[]", string g
                | AllowFreeDownload true -> "allow_free_download", "1"
                | AllowFreeDownload false -> "allow_free_download", "0"
                | AddWatermark true -> "add_watermark", "1"
                | AddWatermark false -> "add_watermark", "0"

            "itemid", string (itemid item)
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/stash/publish"
        |> Utils.readAsync
        |> Utils.thenParse<StashPublishResponse>

    type StashPublishUserdataResult = {
        features: string list
        agreements: string list
    }

    let GetPublishUserdataAsync token =
        Seq.empty
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/stash/publish/userdata"
        |> Utils.readAsync
        |> Utils.thenParse<StashPublishUserdataResult>

    type StashSpaceResult = {
        available_space: int64
        total_space: int64
    }

    let GetSpaceAsync token =
        Seq.empty
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/stash/space"
        |> Utils.readAsync
        |> Utils.thenParse<StashSpaceResult>

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
        static member Default = {
            title = DefaultSubmissionTitle
            artist_comments = NoArtistComments
            tags = TagList.Empty
            original_url = NoOriginalUrl
            is_dirty = false
        }

    type StashSubmitResult = {
        status: string
        itemid: int64
        stack: string
        stackid: int64
    }

    let SubmitAsync token (destination: SubmissionDestination) (parameters: SubmissionParameters) (file: IFormFile) = task {
        let h1 = $"-----------------------------{DateTime.UtcNow.Ticks}"
        let h2 = $"--{h1}"
        let h3 = $"--{h1}--"

        let! multipartBody = async {
            use ms = new MemoryStream()
            let w (s: string) =
                let bytes = System.Text.Encoding.UTF8.GetBytes(sprintf "%s\n" s)
                ms.Write(bytes, 0, bytes.Length)
            
            match parameters.title with
            | SubmissionTitle s ->
                w h2
                w "Content-Disposition: form-data; name=\"title\""
                w ""
                w s
            | DefaultSubmissionTitle -> ()

            match parameters.artist_comments with
            | ArtistComments s ->
                w h2
                w "Content-Disposition: form-data; name=\"artist_comments\""
                w ""
                w s
            | NoArtistComments -> ()

            match parameters.tags with
            | TagList s ->
                let mutable index = 0
                for t in s do
                    w h2
                    w (sprintf "Content-Disposition: form-data; name=\"tags[%d]\"" index)
                    w ""
                    w t
                    index <- index + 1

            match parameters.original_url with
            | OriginalUrl s ->
                w h2
                w "Content-Disposition: form-data; name=\"original_url\""
                w ""
                w s
            | NoOriginalUrl -> ()

            w h2
            w "Content-Disposition: form-data; name=\"is_dirty\""
            w ""
            w (sprintf "%b" parameters.is_dirty)

            match destination with
            | ReplaceExisting (Item itemId) ->
                w h2
                w "Content-Disposition: form-data; name=\"itemid\""
                w ""
                w (sprintf "%d" itemId)
            | SubmitToStackWithName s ->
                w h2
                w "Content-Disposition: form-data; name=\"stack\""
                w ""
                w s
            | SubmitToStack (Stack stackId) ->
                w h2
                w "Content-Disposition: form-data; name=\"stackid\""
                w ""
                w (sprintf "%d" stackId)
            | SubmitToStack RootStack -> ()

            w h2
            w (sprintf "Content-Disposition: form-data; name=\"submission\"; filename=\"%s\"" file.Filename)
            w (sprintf "Content-Type: %s" file.ContentType)
            w ""
            ms.Flush()
            ms.Write(file.Data, 0, file.Data.Length)
            ms.Flush()
            w ""
            w h3

            return ms.ToArray()
        }

        let content = new ByteArrayContent(multipartBody)
        content.Headers.Remove("Content-Type") |> ignore
        content.Headers.Add("Content-Type", $"multipart/form-data; boundary={h1}")

        return! content
        |> Utils.postContent token "https://www.deviantart.com/api/v1/oauth2/stash/submit"
        |> Utils.readAsync
        |> Utils.thenParse<StashSubmitResult>
    }

    type StackModification =
    | ModifyStackTitle of string
    | ModifyStackDescription of string
    | ClearStackDescription

    let UpdateAsync token stackModifications stack =
        seq {
            for s in stackModifications do
                match s with
                | ModifyStackTitle v -> yield "title", v
                | ModifyStackDescription v -> yield "description", v
                | ClearStackDescription -> yield "description", "null"
        }
        |> Utils.post token $"https://www.deviantart.com/api/v1/oauth2/stash/update/{stackid stack}"
        |> Utils.readAsync
        |> Utils.thenParse<SuccessOrErrorResponse>