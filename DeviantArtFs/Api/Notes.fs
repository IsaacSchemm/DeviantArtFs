namespace DeviantArtFs.Api

open System
open DeviantArtFs
open DeviantArtFs.ParameterTypes
open DeviantArtFs.ResponseTypes
open DeviantArtFs.Pages

module Notes =
    type FolderId = FolderId of Guid | Inbox

    type Note = {
        noteid: Guid
        ts: DateTimeOffset
        unread: bool
        starred: bool
        sent: bool
        subject: string
        preview: string
        body: string
        user: User
        recipients: User list
    }

    let PageNotesAsync token folderid limit offset =
        seq {
            match folderid with
            | FolderId f -> "folder", Utils.guidString f
            | Inbox -> ()

            yield! QueryFor.offset offset
            yield! QueryFor.limit limit 24
        }
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/notes"
        |> Utils.readAsync
        |> Utils.thenParse<Page<Note>>

    let GetNotesAsync token folderid batchsize cursor = Utils.buildTaskSeq {
        get_page = (fun cursor -> PageNotesAsync token folderid batchsize cursor)
        extract_data = (fun page -> page.results.Value)
        has_more = (fun page -> page.has_more = Some true)
        extract_next_offset = (fun page -> PagingOffset page.next_offset.Value)
        initial_offset = cursor
    }

    type Folder = {
        folder: Guid
        parentid: Guid option
        title: string
        count: string
    }

    let GetFoldersAsync token =
        Seq.empty
        |> Utils.get token "https://www.deviantart.com/api/v1/oauth2/notes/folders"
        |> Utils.readAsync
        |> Utils.thenParse<ListOnlyResponse<Folder>>
