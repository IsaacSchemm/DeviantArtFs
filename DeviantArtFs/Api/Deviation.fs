namespace DeviantArtFs.Api

open DeviantArtFs
open System
open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages
open FSharp.Control
open FSharp.Json

module Deviation =
    let GetAsync token id =
        Seq.empty
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/deviation/{Utils.guidString id}"
        |> Utils.readAsync
        |> Utils.thenParse<Deviation>

    type TextContent = {
        html: string option
        css: string option
        css_fonts: string list option
    }

    let GetContentAsync token deviationid =
        seq {
            "deviationid", Utils.guidString deviationid
        }
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/deviation/content"
        |> Utils.readAsync
        |> Utils.thenParse<TextContent>

    type Download = {
        src: string
        filename: string
        height: int
        width: int
        filesize: int
    }

    let DownloadAsync token deviationid =
        Seq.empty
        |> Utils.get token $"https://www.deviantart.com/api/v1/oauth2/deviation/download/{Utils.guidString deviationid}"
        |> Utils.readAsync
        |> Utils.thenParse<Download>

    type DeviationCreateResponse = {
        deviationid: Guid
    }

    type DeviationUpdateResponse = {
        status: string
        url: string
        deviationid: Guid
    }

    type MutableDeviationField =
    | Title of string
    | Maturity of Maturity
    | AllowComments of bool
    | GalleryId of Guid
    | AllowFreeDownload of bool
    | AddWatermark of bool
    | License of License
    | Tag of string

    let EditDeviationAsync token deviationid deviationFields =
        seq {
            for f in deviationFields do
                match f with
                | Title x -> "title", x
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
                | AllowComments false -> "allow_comments", "0"
                | AllowComments true -> "allow_comments", "1"
                | GalleryId g ->
                    "galleryids[]", Utils.guidString g
                | AllowFreeDownload false -> "allow_free_download", "0"
                | AllowFreeDownload true -> "allow_free_download", "1"
                | AddWatermark false -> "add_watermark", "0"
                | AddWatermark true -> "add_watermark", "1"
                | License DefaultLicense ->
                    "license_options[creative_commons]", "0"
                | License (CreativeCommons (_, commercialUse, derivativeWorks)) ->
                    "license_options[creative_commons]", "1"
                    match commercialUse with
                    | CC_CommercialUsePermitted -> "license_options[commercial]", "yes"
                    | CC_NonCommercial -> "license_options[commercial]", "no"
                    match derivativeWorks with
                    | CC_DerivativeWorksPermitted -> "license_options[modify]", "yes"
                    | CC_NoDerivatives -> "license_options[modify]", "no"
                    | CC_ShareAlike -> "license_options[modify]", "share"
                | Tag t ->
                    "tags[]", t
        }
        |> Utils.post token $"https://www.deviantart.com/api/v1/oauth2/deviation/edit/{Utils.guidString deviationid}"
        |> Utils.readAsync
        |> Utils.thenParse<DeviationUpdateResponse>

    type EmbeddedDeviationOffset = StartWithFirst | StartWith of Guid with static member Default = StartWithFirst

    type EmbeddedContentPage = {
        has_more: bool
        next_offset: int option
        has_less: bool option
        prev_offset: int option
        results: Deviation list
    }

    let PageEmbeddedContentAsync token deviationid offset_deviationid limit offset =
        seq {
            yield "deviationid", Utils.guidString deviationid
            match offset_deviationid with
            | StartWithFirst -> ()
            | StartWith g -> yield "offset_deviationid", string g
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/deviation/embeddedcontent"
        |> Utils.readAsync
        |> Utils.thenParse<EmbeddedContentPage>

    let GetEmbeddedContentAsync token deviationid batchsize offset = taskSeq {
        let mutable offset = offset
        let mutable has_more = true
        while has_more do
            let! data = PageEmbeddedContentAsync token deviationid StartWithFirst batchsize offset
            yield! data.results
            has_more <- data.has_more
            if has_more then
                offset <- PagingOffset data.next_offset.Value
    }

    module Journal =
        type MutableField =
        | Title of string
        | Tag of string
        | CoverImageDeviationId of Guid
        | ResetCoverImageDeviationId
        | IsMature of bool
        | AllowComments of bool
        | License of License

        type ImmutableField =
        | Body of string
        | EmbeddedImageDeviationId of Guid

        let CreateAsync token immutableFields mutableFields =
            seq {
                for f in immutableFields do
                    match f with
                    | Body x ->
                        "body", x
                    | EmbeddedImageDeviationId c ->
                        "embedded_image_deviation_id", Utils.guidString c
                for f in mutableFields do
                    match f with
                    | Title t ->
                        "title", t
                    | Tag t ->
                        "tags[]", t
                    | CoverImageDeviationId c ->
                        "cover_image_deviation_id", Utils.guidString c
                    | ResetCoverImageDeviationId ->
                        ()
                    | IsMature m ->
                        "is_mature", if m then "1" else "0"
                    | AllowComments a ->
                        "allow_comments", if a then "1" else "0"
                    | License DefaultLicense ->
                        "license_options[creative_commons]", "0"
                    | License (CreativeCommons (_, commercialUse, derivativeWorks)) ->
                        "license_options[creative_commons]", "1"
                        match commercialUse with
                        | CC_CommercialUsePermitted -> "license_options[commercial]", "yes"
                        | CC_NonCommercial -> "license_options[commercial]", "no"
                        match derivativeWorks with
                        | CC_DerivativeWorksPermitted -> "license_options[modify]", "yes"
                        | CC_NoDerivatives -> "license_options[modify]", "no"
                        | CC_ShareAlike -> "license_options[modify]", "share"
            }
            |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/deviation/journal/create"
            |> Utils.readAsync
            |> Utils.thenParse<DeviationCreateResponse>

        let UpdateAsync token deviationid mutableFields =
            seq {
                for f in mutableFields do
                    match f with
                    | Title t ->
                        "title", t
                    | Tag t ->
                        "tags[]", t
                    | CoverImageDeviationId c ->
                        "cover_image_deviation_id", Utils.guidString c
                    | ResetCoverImageDeviationId ->
                        "reset_cover_image_deviation_id", "1"
                    | IsMature m ->
                        "is_mature", if m then "1" else "0"
                    | AllowComments a ->
                        "allow_comments", if a then "1" else "0"
                    | License DefaultLicense ->
                        "license_options[creative_commons]", "0"
                    | License (CreativeCommons (_, commercialUse, derivativeWorks)) ->
                        "license_options[creative_commons]", "1"
                        match commercialUse with
                        | CC_CommercialUsePermitted -> "license_options[commercial]", "yes"
                        | CC_NonCommercial -> "license_options[commercial]", "no"
                        match derivativeWorks with
                        | CC_DerivativeWorksPermitted -> "license_options[modify]", "yes"
                        | CC_NoDerivatives -> "license_options[modify]", "no"
                        | CC_ShareAlike -> "license_options[modify]", "share"
            }
            |> Utils.post token $"https://www.deviantart.com/api/v1/oauth2/deviation/journal/update/{Utils.guidString deviationid}"
            |> Utils.readAsync
            |> Utils.thenParse<DeviationUpdateResponse>

    module Literature =
        type MutableField =
        | Title of string
        | Tag of string
        | GalleryId of Guid
        | Maturity of Maturity
        | AllowComments of bool
        | License of License

        type ImmutableField =
        | Body of string
        | Description of string
        | EmbeddedImageDeviationId of Guid

        let CreateAsync token immutableFields mutableFields =
            seq {
                for f in immutableFields do
                    match f with
                    | Body x ->
                        "body", x
                    | Description x ->
                        "description", x
                    | EmbeddedImageDeviationId c ->
                        "embedded_image_deviation_id", Utils.guidString c
                for f in mutableFields do
                    match f with
                    | Title t ->
                        "title", t
                    | Tag t ->
                        "tags[]", t
                    | GalleryId g ->
                        "galleryids[]", Utils.guidString g
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
                    | AllowComments a ->
                        "allow_comments", if a then "1" else "0"
                    | License DefaultLicense ->
                        "license_options[creative_commons]", "0"
                    | License (CreativeCommons (_, commercialUse, derivativeWorks)) ->
                        "license_options[creative_commons]", "1"
                        match commercialUse with
                        | CC_CommercialUsePermitted -> "license_options[commercial]", "yes"
                        | CC_NonCommercial -> "license_options[commercial]", "no"
                        match derivativeWorks with
                        | CC_DerivativeWorksPermitted -> "license_options[modify]", "yes"
                        | CC_NoDerivatives -> "license_options[modify]", "no"
                        | CC_ShareAlike -> "license_options[modify]", "share"
            }
            |> Utils.post token "https://www.deviantart.com/api/v1/oauth2/deviation/literature/create"
            |> Utils.readAsync
            |> Utils.thenParse<DeviationCreateResponse>

        let UpdateAsync token deviationid mutableFields =
            seq {
                for f in mutableFields do
                    match f with
                    | Title t ->
                        "title", t
                    | Tag t ->
                        "tags[]", t
                    | GalleryId g ->
                        "galleryids[]", Utils.guidString g
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
                    | AllowComments a ->
                        "allow_comments", if a then "1" else "0"
                    | License DefaultLicense ->
                        "license_options[creative_commons]", "0"
                    | License (CreativeCommons (_, commercialUse, derivativeWorks)) ->
                        "license_options[creative_commons]", "1"
                        match commercialUse with
                        | CC_CommercialUsePermitted -> "license_options[commercial]", "yes"
                        | CC_NonCommercial -> "license_options[commercial]", "no"
                        match derivativeWorks with
                        | CC_DerivativeWorksPermitted -> "license_options[modify]", "yes"
                        | CC_NoDerivatives -> "license_options[modify]", "no"
                        | CC_ShareAlike -> "license_options[modify]", "share"
            }
            |> Utils.post token $"https://www.deviantart.com/api/v1/oauth2/deviation/journal/literature/{Utils.guidString deviationid}"
            |> Utils.readAsync
            |> Utils.thenParse<DeviationUpdateResponse>

    type Tag = {
        tag_name: string
        sponsored: bool
        sponsor: string option
    }

    type MetadataSubmission = {
        creation_time: DateTimeOffset
        category: string
        file_size: string option
        resolution: string option
        submitted_with: SubmittedWith
    }

    type MetadataStats = {
        views: int
        views_today: int
        favourites: int
        comments: int
        downloads: int
        downloads_today: int
    }

    type Metadata = {
        deviationid: Guid
        printid: Guid option
        author: User
        is_watching: bool
        title: string
        description: string
        license: string
        allows_comments: bool
        tags: Tag list
        is_favourited: bool
        is_mature: bool
        submission: MetadataSubmission option
        stats: MetadataStats option
        camera: Map<string, string> option
        collections: CollectionFolder list option
        can_post_comment: bool
    }

    type MetadataResponse = {
        metadata: Metadata list
    }

    let GetMetadataAsync token deviationids =
        seq {
            for id in deviationids do
                yield "deviationids[]", Utils.guidString id
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/deviation/metadata"
        |> Utils.readAsync
        |> Utils.thenParse<MetadataResponse>

    type WhoFavedUser = {
        user: User
        [<JsonField(Transform=typeof<Transforms.DateTimeOffsetEpoch>)>]
        time: DateTimeOffset
    }

    let PageWhoFavedAsync token deviationid limit offset =
        seq {
            yield "deviationid", Utils.guidString deviationid
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/deviation/whofaved"
        |> Utils.readAsync
        |> Utils.thenParse<Page<WhoFavedUser>>

    let GetWhoFavedAsync token req batchsize offset = taskSeq {
        let mutable offset = offset
        let mutable has_more = true
        while has_more do
            let! data = PageWhoFavedAsync token req batchsize offset
            yield! data.results.Value
            has_more <- data.has_more.Value
            if has_more then
                offset <- PagingOffset data.next_offset.Value
    }