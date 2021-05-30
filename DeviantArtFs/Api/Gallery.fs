namespace DeviantArtFs.Api

open System
open DeviantArtFs
open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages

module Gallery =
    let AsyncPageAllView token scope limit offset =
        seq {
            yield! QueryFor.userScope scope
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 24
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/gallery/all"
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<Deviation>>

    let AsyncGetAllView token scope batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPageAllView token scope batchsize)

    let AsyncPageGallery token expansion scope folder limit offset =
        let folder_id_str =
            match folder with
            | SingleGalleryFolder s -> sprintf "%O" s
            | AllGalleryFoldersNewest -> ""
            | AllGalleryFoldersPopular -> ""

        seq {
            yield! QueryFor.userScope scope
            match folder with
            | SingleGalleryFolder s -> ()
            | AllGalleryFoldersNewest -> "mode=newest"
            | AllGalleryFoldersPopular -> "mode=popular"
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 24
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/%s" folder_id_str)
        |> Dafs.asyncRead
        |> Dafs.thenParse<FolderPage>

    let AsyncGetGallery token expansion scope folder batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPageGallery token expansion scope folder batchsize)

    let AsyncPageFolders token calculateSize extPreload user limit offset =
        seq {
            yield! QueryFor.userScope user
            yield! QueryFor.calculateSize calculateSize
            yield! QueryFor.folderPreload extPreload
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/gallery/folders"
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<GalleryFolder>>

    let AsyncGetFolders token calculateSize extPreload user batchsize offset =
        Dafs.toAsyncEnum offset (AsyncPageFolders token calculateSize extPreload user batchsize)

    let AsyncCreateFolder token folder =
        seq {
            yield sprintf "folder=%s" (Uri.EscapeDataString folder)
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/gallery/folders/create"
        |> Dafs.asyncRead
        |> Dafs.thenParse<CollectionFolder>

    let AsyncRemoveFolder token folderid =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/folders/remove/%s" (Dafs.guid2str folderid))
        |> Dafs.asyncRead
        |> Dafs.thenParse<SuccessOrErrorResponse>