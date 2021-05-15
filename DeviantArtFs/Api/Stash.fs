namespace DeviantArtFs.Api

open DeviantArtFs
open System
open System.IO

module Stash =
    let AsyncGetStack token (stackid: int64) =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/%d" stackid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashMetadata>

    let RootStack = 0L

    let AsyncPageContents token (stackid: int64) paging =
        seq {
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/%d/contents" stackid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<StashMetadata>>

    let AsyncGetContents token stackid offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageContents token stackid)

    let AsyncDelete token (itemid: int64) =
        seq {
            yield sprintf "itemid=%d" itemid
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/stash/delete"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>

    type DeltaRequest() = 
        member val Cursor = null with get, set
        member val ExtParams = ParameterTypes.ExtParams.None with get, set

    let AsyncPageDelta token (req: DeltaRequest) paging =
        seq {
            match Option.ofObj req.Cursor with
            | Some s -> yield sprintf "cursor=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 120
            yield! QueryFor.extParams req.ExtParams
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/stash/delta"
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashDeltaResult>

    let AsyncGetDelta token req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageDelta token req)

    type ItemRequest(itemid: int64) = 
        member __.Itemid = itemid
        member val ExtParams = ParameterTypes.ExtParams.None with get, set

    let AsyncGetItem token (req: ItemRequest) =
        seq {
            yield! QueryFor.extParams req.ExtParams
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/item/%d" req.Itemid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashMetadata>

    let AsyncMoveItem token (stackid: int64) (targetid: int64) =
        seq {
            yield sprintf "targetid=%d" targetid
        }
        |> Dafs.createRequest Dafs.Method.POST token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/move/%d" stackid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashMoveResult>

    let AsyncPositionItem token (stackid: int64) (position: int) =
        seq {
            yield sprintf "position=%d" position
        }
        |> Dafs.createRequest Dafs.Method.POST token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/position/%d" stackid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>

    [<RequireQualifiedAccess>]
    type LicenseModifyOption = No | Yes | ShareAlike

    [<RequireQualifiedAccess>]
    type MatureLevel = None | Strict | Moderate

    [<RequireQualifiedAccess>]
    type MatureClassification = Nudity | Sexual | Gore | Language | Ideology

    [<RequireQualifiedAccess>]
    type Sharing = Allow | HideShareButtons | HideAndMembersOnly

    type DisplayResolution = Original=0 | Max400Px=1 | Max600px=2 | Max800px=3 | Max900px=4 | Max1024px=5 | Max1280px=6 | Max1600px=7 | Max1920px=8

    type LicenseOptions() =
        member val CreativeCommons = false with get, set
        member val Commercial = false with get, set
        member val Modify = LicenseModifyOption.No with get, set

    type PublishRequest(itemid: int64) =
        member val IsMature = false with get, set
        member val MatureLevel = MatureLevel.None with get, set
        member val MatureClassification = Seq.empty<MatureClassification> with get, set
        member val AgreeSubmission = false with get, set
        member val AgreeTos = false with get, set
        member val Catpath = null with get, set
        member val Feature = false with get, set
        member val AllowComments = false with get, set
        member val RequestCritique = false with get, set
        member val DisplayResolution = DisplayResolution.Original with get, set
        member val Sharing = Sharing.Allow with get, set
        member val LicenseOptions = new LicenseOptions() with get, set
        member val Galleryids = Seq.empty<Guid> with get, set
        member val AllowFreeDownload = false with get, set
        member val AddWatermark = false with get, set
        member val Itemid = itemid

    let AsyncPublish token (req: PublishRequest) =
        seq {
            yield sprintf "is_mature=%b" req.IsMature
            match req.MatureLevel with
            | MatureLevel.Strict -> yield "mature_level=strict"
            | MatureLevel.Moderate -> yield "mature_level=moderate"
            | _ -> ()
            if req.MatureClassification |> Seq.contains MatureClassification.Nudity then
                yield "mature_classification[]=nudity"
            if req.MatureClassification |> Seq.contains MatureClassification.Sexual then
                yield "mature_classification[]=sexual"
            if req.MatureClassification |> Seq.contains MatureClassification.Gore then
                yield "mature_classification[]=gore"
            if req.MatureClassification |> Seq.contains MatureClassification.Language then
                yield "mature_classification[]=language"
            if req.MatureClassification |> Seq.contains MatureClassification.Ideology then
                yield "mature_classification[]=ideology"
            yield sprintf "agree_submission=%b" req.AgreeSubmission
            yield sprintf "agree_tos=%b" req.AgreeTos
            yield sprintf "catpath=%s" (Dafs.urlEncode req.Catpath)
            yield sprintf "feature=%b" req.Feature
            yield sprintf "allow_comments=%b" req.AllowComments
            yield sprintf "request_critique=%b" req.RequestCritique
            yield sprintf "display_resolution=%d" (int req.DisplayResolution)
            match req.Sharing with
            | Sharing.Allow -> yield sprintf "sharing=allow"
            | Sharing.HideShareButtons -> yield sprintf "sharing=hide_share_buttons"
            | Sharing.HideAndMembersOnly -> yield sprintf "sharing=hide_and_members_only"
            yield sprintf "license_options[creative_commons]=%b" req.LicenseOptions.CreativeCommons
            yield sprintf "license_options[commercial]=%s" (if req.LicenseOptions.Commercial then "yes" else "no")
            match req.LicenseOptions.Modify with
            | LicenseModifyOption.Yes -> yield "license_options[modify]=yes"
            | LicenseModifyOption.No -> yield "license_options[modify]=no"
            | LicenseModifyOption.ShareAlike -> yield "license_options[modify]=share"
            for id in req.Galleryids do
                yield sprintf "galleryids[]=%O" id
            yield sprintf "allow_free_download=%b" req.AllowFreeDownload
            yield sprintf "add_watermark=%b" req.AddWatermark
            yield sprintf "itemid=%d" req.Itemid
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/stash/publish"
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashPublishResponse>

    type PublishCategoryTreeRequest(catpath: string) = 
        member __.Catpath = catpath
        member val Filetype = null with get, set
        member val Frequent = false with get, set

    let AsyncGetPublishCategoryTree token (req: PublishCategoryTreeRequest) =
        seq {
            yield sprintf "catpath=%s" (Dafs.urlEncode req.Catpath)
            if not (isNull req.Filetype) then
                yield sprintf "filetype=%s" (Dafs.urlEncode req.Filetype)
            yield sprintf "frequent=%b" req.Frequent
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/stash/publish/categorytree"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtCategoryList>

    let AsyncGetPublishUserdata token =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/stash/publish/userdata"
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashPublishUserdataResult>

    let AsyncGetSpace token =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/stash/space"
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashSpaceResult>

    type SubmitRequest(filename: string, contentType: string, data: byte[]) =
        member __.Filename = filename
        member __.ContentType = contentType
        member __.Data = data
        member val Title = null with get, set
        member val ArtistComments = null with get, set
        member val Tags = Seq.empty with get, set
        member val OriginalUrl = null with get, set
        member val IsDirty = Nullable<bool>() with get, set
        member val Itemid = Nullable<int64>() with get, set
        member val Stack = null with get, set
        member val Stackid = Nullable<int64>() with get, set

    let AsyncSubmit token (ps: SubmitRequest) = async {
        // multipart separators
        let h1 = sprintf "-----------------------------%d" DateTime.UtcNow.Ticks
        let h2 = sprintf "--%s" h1
        let h3 = sprintf "--%s--" h1

        let req = Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/stash/submit" Seq.empty
        req.Method <- "POST"
        req.ContentType <- sprintf "multipart/form-data; boundary=%s" h1

        do! async {
            use ms = new MemoryStream()
            let w (s: string) =
                let bytes = System.Text.Encoding.UTF8.GetBytes(sprintf "%s\n" s)
                ms.Write(bytes, 0, bytes.Length)
            
            match Option.ofObj ps.Title with
            | Some s ->
                w h2
                w "Content-Disposition: form-data; name=\"title\""
                w ""
                w s
            | None -> ()

            match Option.ofObj ps.ArtistComments with
            | Some s ->
                w h2
                w "Content-Disposition: form-data; name=\"artist_comments\""
                w ""
                w s
            | None -> ()

            match Option.ofObj ps.Tags with
            | Some s ->
                let mutable index = 0
                for t in s do
                    w h2
                    w (sprintf "Content-Disposition: form-data; name=\"tags[%d]\"" index)
                    w ""
                    w t
                    index <- index + 1
            | None -> ()

            match Option.ofObj ps.OriginalUrl with
            | Some s ->
                w h2
                w "Content-Disposition: form-data; name=\"original_url\""
                w ""
                w s
            | None -> ()

            match Option.ofNullable ps.IsDirty with
            | Some s ->
                w h2
                w "Content-Disposition: form-data; name=\"is_dirty\""
                w ""
                w (sprintf "%b" s)
            | None -> ()

            match Option.ofNullable ps.Itemid with
            | Some s ->
                w h2
                w "Content-Disposition: form-data; name=\"itemid\""
                w ""
                w (sprintf "%d" s)
            | None -> ()

            match Option.ofObj ps.Stack with
            | Some s ->
                w h2
                w "Content-Disposition: form-data; name=\"stack\""
                w ""
                w s
            | None -> ()

            match Option.ofNullable ps.Stackid with
            | Some s ->
                w h2
                w "Content-Disposition: form-data; name=\"stackid\""
                w ""
                w (sprintf "%d" s)
            | None -> ()

            w h2
            w (sprintf "Content-Disposition: form-data; name=\"submission\"; filename=\"%s\"" ps.Filename)
            w (sprintf "Content-Type: %s" ps.ContentType)
            w ""
            ms.Flush()
            ms.Write(ps.Data, 0, ps.Data.Length)
            ms.Flush()
            w ""
            w h3

            req.RequestBody <- ms.ToArray()
        }

        return! req
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashSubmitResult>
    }
    
    [<RequireQualifiedAccess>]
    type UpdateField = Title of string | Description of string | ClearDescription

    let AsyncUpdate token (stackid: int64) (updates: UpdateField seq) =
        seq {
            for update in updates do
                match update with
                | UpdateField.Title v -> yield sprintf "title=%s" (Dafs.urlEncode v)
                | UpdateField.Description v -> yield sprintf "description=%s" (Dafs.urlEncode v)
                | UpdateField.ClearDescription -> yield "description=null"
        }
        |> Dafs.createRequest Dafs.Method.POST token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/update/%d" stackid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>