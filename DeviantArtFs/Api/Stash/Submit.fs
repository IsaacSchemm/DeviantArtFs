namespace DeviantArtFs.Api.Stash

open DeviantArtFs
open System
open System.IO

type SubmitResponse = {
    status: string
    itemid: int64
    stack: string
    stackid: int64
}

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

module Submit =
    let AsyncExecute token (common: DeviantArtCommonParams) (ps: SubmitRequest) = async {
        // multipart separators
        let h1 = sprintf "-----------------------------%d" DateTime.UtcNow.Ticks
        let h2 = sprintf "--%s" h1
        let h3 = sprintf "--%s--" h1

        let req = Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/stash/submit" Seq.empty
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
            w "Content-Disposition: form-data; name=\"mature_content\""
            w ""
            match common.MatureContent with
            | true ->
                w "true"
            | false ->
                w "false"

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
        |> Dafs.thenParse<SubmitResponse>
    }

    let ExecuteAsync token common ps =
        AsyncExecute token common ps
        |> Async.StartAsTask