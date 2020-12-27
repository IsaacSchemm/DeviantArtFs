namespace DeviantArtFs.Api.Gallery

open System
open DeviantArtFs
open FSharp.Control

type GalleryRequestMode = Popular | Newest

type GalleryByIdRequest() =
    member val Folderid = Nullable<Guid>() with get, set
    member val Username = null with get, set
    member val Mode = GalleryRequestMode.Popular with get, set

module GalleryById =
    let AsyncExecute token expansion (req: GalleryByIdRequest) paging =
        let folder_id_str =
            match Option.ofNullable req.Folderid with
            | Some s -> sprintf "%O" s
            | None -> ""

        seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (Dafs.urlEncode s)
            | None -> ()
            yield sprintf "mode=%s" (if req.Mode = GalleryRequestMode.Newest then "newest" else "popular")
            yield! QueryFor.paging paging 24
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/%s" folder_id_str)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtFolderPagedResult>

    let ToAsyncSeq token expansion req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncExecute token expansion req)

    let ToArrayAsync token expansion req offset limit =
        ToAsyncSeq token expansion req offset
        |> AsyncSeq.take limit
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask

    let ExecuteAsync token expansion req paging =
        AsyncExecute token expansion req paging
        |> Async.StartAsTask