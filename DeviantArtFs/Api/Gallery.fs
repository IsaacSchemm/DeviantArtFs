namespace DeviantArtFs.Api

open System
open DeviantArtFs

module Gallery =
    type GalleryAllViewRequest() =
        member val Username = null with get, set

    let AsyncPageAllView token (req: GalleryAllViewRequest) paging =
        seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (Dafs.urlEncode s)
            | None -> ()
            yield! QueryFor.paging paging 24
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/gallery/all"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<Deviation>>

    let AsyncGetAllView token req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageAllView token req)

    type GalleryRequestMode = PopularGalleryContents | NewestGalleryContents

    type GalleryRequest() =
        member val Folderid = Nullable<Guid>() with get, set
        member val Username = null with get, set
        member val Mode = PopularGalleryContents with get, set

    let AsyncPageGallery token expansion (req: GalleryRequest) paging =
        let folder_id_str =
            match Option.ofNullable req.Folderid with
            | Some s -> sprintf "%O" s
            | None -> ""

        seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (Dafs.urlEncode s)
            | None -> ()
            yield sprintf "mode=%s" (match req.Mode with NewestGalleryContents -> "newest" | PopularGalleryContents -> "popular")
            yield! QueryFor.paging paging 24
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/%s" folder_id_str)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtFolderPagedResult>

    let AsyncGetGallery token expansion req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageGallery token expansion req)

    type GalleryFoldersRequest() =
        member val Username = null with get, set
        member val CalculateSize = false with get, set
        member val ExtPreload = false with get, set

    let AsyncPageFolders token (req: GalleryFoldersRequest) paging =
        seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (Dafs.urlEncode s)
            | None -> ()
            yield sprintf "calculate_size=%b" req.CalculateSize
            yield sprintf "ext_preload=%b" req.ExtPreload
            yield! QueryFor.paging paging 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/gallery/folders"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtPagedResult<DeviantArtGalleryFolder>>

    let AsyncGetFolders token req offset =
        Dafs.toAsyncSeq (DeviantArtPagingParams.MaxFrom offset) (AsyncPageFolders token req)

    let AsyncCreateFolder token (folder: string) =
        seq {
            yield sprintf "folder=%s" (Dafs.urlEncode folder)
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/gallery/folders/create"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtCollectionFolder>

    let AsyncRemoveFolder token (folderid: Guid) =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/folders/remove/%A" folderid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviantArtSuccessOrErrorResponse>