namespace DeviantArtFs.Api

open DeviantArtFs
open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages

module Collections =
    let AsyncPageCollection token expansion user folderid limit offset =
        seq {
            yield! QueryFor.collectionsUser user
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 24
            yield! QueryFor.objectExpansion expansion
        }
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/collections/%s" (Dafs.guid2str folderid))
        |> Dafs.asyncRead
        |> Dafs.thenParse<FolderPage>

    let AsyncGetCollection token expansion user folderid batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPageCollection token expansion user folderid batchsize)

    let AsyncPageFolders token calculateSize extPreload username limit offset =
        seq {
            yield! QueryFor.collectionsUser username
            yield! QueryFor.collectionsFolderCalculateSize calculateSize
            yield! QueryFor.collectionsFolderPreload extPreload
            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 50
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/collections/folders"
        |> Dafs.asyncRead
        |> Dafs.thenParse<Page<CollectionFolder>>

    let AsyncGetFolders token extPreload calculateSize username batchsize offset =
        Dafs.toAsyncSeq offset (AsyncPageFolders token extPreload calculateSize username batchsize)

    let AsyncFave token deviationid folderids =
        seq {
            yield sprintf "deviationid=%s" (Dafs.guid2str deviationid)
            let mutable index = 0
            for f in folderids do
                yield sprintf "folderid[%d]=%O" index (Dafs.guid2str f)
                index <- index + 1
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/collections/fave"
        |> Dafs.asyncRead
        |> Dafs.thenParse<FaveResult>

    let AsyncUnfave token deviationid folderids =
        seq {
            yield sprintf "deviationid=%s" (Dafs.guid2str deviationid)
            let mutable index = 0
            for f in folderids do
                yield sprintf "folderid[%d]=%O" index (Dafs.guid2str f)
                index <- index + 1
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/collections/unfave"
        |> Dafs.asyncRead
        |> Dafs.thenParse<FaveResult>

    let AsyncCreateFolder token folder =
        seq {
            yield sprintf "folder=%s" (System.Uri.EscapeDataString folder)
        }
        |> Dafs.createRequest Dafs.Method.POST token "https://www.deviantart.com/api/v1/oauth2/collections/folders/create"
        |> Dafs.asyncRead
        |> Dafs.thenParse<CollectionFolder>

    let AsyncRemoveFolder token folderid =
        Seq.empty
        |> Dafs.createRequest Dafs.Method.GET token (sprintf "https://www.deviantart.com/api/v1/oauth2/collections/folders/remove/%s" (Dafs.guid2str folderid))
        |> Dafs.asyncRead
        |> Dafs.thenParse<SuccessOrErrorResponse>