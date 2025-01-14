﻿namespace DeviantArtFs.Api

open System
open System.IO
open DeviantArtFs
open DeviantArtFs.ParameterTypes
open System.Net.Http
open FSharp.Control

module Stash =
    type Stack = Stack of int64 | RootStack
    type Item = Item of int64

    let private itemid item = match item with Item x -> x

    type DeltaCursor = DeltaCursor of string | Initial

    type SubmissionDestination =
    | ReplaceExisting of Item
    | SubmitToStack of Stack
    | SubmitToStackWithName of string
    with static member Default = SubmitToStack RootStack

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

    type SubjectTagType =
    | Artist = 5
    | Model = 1
    | Object = 3
    | Character = 4

    type Group = Group of Guid
    type GroupFolder = GroupFolder of Guid

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
    | Tag of string
    | SubjectTag of string * SubjectTagType
    | LocationTag of string
    | Group of Group * GroupFolder
    | IsAiGenerated
    | IsNotAiGenerated
    | NoThirdPartyAi
    | ThirdPartyAiOk

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
                | License (CreativeCommons restrictionSet) ->
                    "license_options[creative_commons]", "1"
                    "license_options[commercial]", match restrictionSet.commercial with CommericalNo -> "no" | CommericalYes -> "yes"
                    "license_options[modify]", match restrictionSet.modify with ModifyNo -> "no" | ModifyShare -> "share" | ModifyYes -> "yes"
                | GalleryId g ->
                    "galleryids[]", string g
                | AllowFreeDownload true -> "allow_free_download", "1"
                | AllowFreeDownload false -> "allow_free_download", "0"
                | AddWatermark true -> "add_watermark", "1"
                | AddWatermark false -> "add_watermark", "0"
                | Tag tag ->
                    "tags[]", tag
                | SubjectTag (tag, tagType) ->
                    "subject_tags[]", tag
                    "subject_tag_types[]", $"{int tagType}"
                | LocationTag tag ->
                    "location_tag", tag
                | Group (group, groupFolder) ->
                    "groups[]", string group
                    "group_folders[]", string groupFolder
                | IsAiGenerated -> "is_ai_generated", "1"
                | IsNotAiGenerated -> "is_ai_generated", "0"
                | NoThirdPartyAi -> "noai", "1"
                | ThirdPartyAiOk -> "noai", "0"

            "itemid", string (itemid item)
        }
        |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/stash/publish"
        |> Utils.readAsync
        |> Utils.thenParse<StashPublishResponse>

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
