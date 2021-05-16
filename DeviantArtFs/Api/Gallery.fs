namespace DeviantArtFs.Api

open System
open DeviantArtFs
open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages

module Gallery =
    type GalleryAllViewRequest() =
        member val Username = null with get, set

    let AsyncPageAllView token (req: GalleryAllViewRequest) limit offset =
        seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (Uri.EscapeDataString s)
            | None -> ()
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 24
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/gallery/all"
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<Deviation>>

    let AsyncGetAllView token req batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPageAllView token req batchsize)

    type GalleryRequestMode = PopularGalleryContents | NewestGalleryContents

    type GalleryRequest() =
        member val Folderid = Nullable<Guid>() with get, set
        member val Username = null with get, set
        member val Mode = PopularGalleryContents with get, set

    let AsyncPageGallery token expansion (req: GalleryRequest) limit offset =
        let folder_id_str =
            match Option.ofNullable req.Folderid with
            | Some s -> sprintf "%O" s
            | None -> ""

        seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (Uri.EscapeDataString s)
            | None -> ()
            yield sprintf "mode=%s" (match req.Mode with NewestGalleryContents -> "newest" | PopularGalleryContents -> "popular")
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 24
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/%s" folder_id_str)
        |> Dafs.asyncRead
        |> Dafs.thenParse<FolderPage>

    let AsyncGetGallery token expansion req batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPageGallery token expansion req batchsize)

    type GalleryFoldersRequest() =
        member val Username = null with get, set
        member val CalculateSize = false with get, set
        member val ExtPreload = false with get, set

    let AsyncPageFolders token (req: GalleryFoldersRequest) limit offset =
        seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (Uri.EscapeDataString s)
            | None -> ()
            yield sprintf "calculate_size=%b" req.CalculateSize
            yield sprintf "ext_preload=%b" req.ExtPreload
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/gallery/folders"
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<GalleryFolder>>

    let AsyncGetFolders token req batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPageFolders token req batchsize)

    let AsyncCreateFolder token (folder: string) =
        seq {
            yield sprintf "folder=%s" (Uri.EscapeDataString folder)
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/gallery/folders/create"
        |> Dafs.asyncRead
        |> Dafs.thenParse<CollectionFolder>

    let AsyncRemoveFolder token (folderid: Guid) =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/folders/remove/%A" folderid)
        |> Dafs.asyncRead
        |> Dafs.thenParse<SuccessOrErrorResponse>