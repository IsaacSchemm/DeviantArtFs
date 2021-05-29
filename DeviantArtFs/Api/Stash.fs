namespace DeviantArtFs.Api

open System
open System.IO
open DeviantArtFs
open DeviantArtFs.ParameterTypes
open DeviantArtFs.SubmissionTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages

module Stash =
    let AsyncGetStack token stack =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/%d" (StashStack.id stack))
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashMetadata>

    let AsyncPageContents token stack limit offset =
        seq {
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/%d/contents" (StashStack.id stack))
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<StashMetadata>>

    let AsyncGetContents token stack batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPageContents token stack batchsize)

    let AsyncDelete token item =
        seq {
            yield! QueryFor.stashItem item
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/stash/delete"
        |> Dafs.asyncRead
        |> Dafs.thenParse<SuccessOrErrorResponse>

    let AsyncPageDelta token extParams cursor limit offset =
        seq {
            yield! QueryFor.stashDeltaCursor cursor
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 120
            yield! QueryFor.extParams extParams
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/stash/delta"
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashDelta>

    let AsyncGetDelta token extParams cursor batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPageDelta token extParams cursor batchsize)

    type ItemRequest(itemid: int64) = 
        member __.Itemid = itemid
        member val ExtParams = ParameterTypes.ExtParams.None with get, set

    let AsyncGetItem token extParams item =
        seq {
            yield! QueryFor.extParams extParams
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/item/%d" (StashItem.id item))
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashMetadata>

    let AsyncMoveItem token stack (targetid: int64) =
        seq {
            yield sprintf "targetid=%d" targetid
        }
        |> Dafs.createRequest Dafs.Method.POST token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/move/%d" (StashStack.id stack))
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashMoveResult>

    let AsyncPositionItem token stack (position: int) =
        seq {
            yield sprintf "position=%d" position
        }
        |> Dafs.createRequest Dafs.Method.POST token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/position/%d" (StashStack.id stack))
        |> Dafs.asyncRead
        |> Dafs.thenParse<SuccessOrErrorResponse>

    let AsyncPublish token parameters item =
        seq {
            yield! QueryFor.publishParameters parameters
            yield! QueryFor.stashItem item
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/stash/publish"
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashPublishResponse>

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

    let AsyncSubmit token (destination: SubmissionDestination) (parameters: SubmissionParameters) (file: IFormFile) = async {
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
            | ReplaceExisting (StashItem itemId) ->
                w h2
                w "Content-Disposition: form-data; name=\"itemid\""
                w ""
                w (sprintf "%d" itemId)
            | SubmitToStackWithName s ->
                w h2
                w "Content-Disposition: form-data; name=\"stack\""
                w ""
                w s
            | SubmitToStack (StashStack stackId) ->
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

            req.RequestBody <- ms.ToArray()
        }

        return! req
        |> Dafs.asyncRead
        |> Dafs.thenParse<StashSubmitResult>
    }

    let AsyncUpdate token stackModifications stack =
        seq {
            for s in stackModifications do
               yield! QueryFor.stackModification s
        }
        |> Dafs.createRequest Dafs.Method.POST token (sprintf "https://www.deviantart.com/api/v1/oauth2/stash/update/%d" (StashStack.id stack))
        |> Dafs.asyncRead
        |> Dafs.thenParse<SuccessOrErrorResponse>