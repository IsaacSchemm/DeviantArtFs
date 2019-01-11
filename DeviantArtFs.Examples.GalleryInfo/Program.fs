open System
open System.IO
open DeviantArtFs

// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

let create_token_obj str = { new IDeviantArtAccessToken with member __.AccessToken = str }

let get_token =
    let token_file = "token.txt"
    let token_string =
        if File.Exists token_file
        then File.ReadAllText token_file
        else ""

    let valid = create_token_obj token_string |> DeviantArtFs.Requests.Util.Placebo.AsyncIsValid |> Async.RunSynchronously
    if valid then
        token_string
    else
        printf "Please enter the client ID (e.g. 1234): "
        let client_id = System.Console.ReadLine() |> Int32.Parse

        printf "Please enter the redirect URL (default: https://www.example.com): "
        let url1 = System.Console.ReadLine()
        let url2 =
            match url1 with
            | "" -> "https://www.example.com"
            | s -> s

        use form = new DeviantArtFs.WinForms.DeviantArtImplicitGrantForm(client_id, new Uri(url2), ["browse"; "user"; "stash"; "publish"; "user.manage"])
        if form.ShowDialog() <> System.Windows.Forms.DialogResult.OK then
            failwithf "Login cancelled"
        else
            File.WriteAllText("token.txt", form.AccessToken)
            form.AccessToken

let page offset limit = new PagingParams(Offset = offset, Limit = Nullable limit)

let sandbox token_string = async {
    let token = create_token_obj token_string

    printf "Enter a username (leave blank to see your own submissions): "
    let read = Console.ReadLine()

    let! me = DeviantArtFs.Requests.User.Whoami.AsyncExecute token

    let username =
        match read with
        | "" -> me.Username
        | s -> s

    let! statuses =
        new DeviantArtFs.Requests.User.StatusesListRequest(username, Offset = 0, Limit = 1)
        |> DeviantArtFs.Requests.User.StatusesList.AsyncExecute token
    let status = Seq.tryHead statuses.Results
    match status with
    | Some s -> 
        printfn "Most recent status: %s (%O)" s.Body s.Ts
        printfn ""
    | None -> ()

    printfn "Gallery folders include:"
    printfn ""

    let! folders = new DeviantArtFs.Requests.Gallery.GalleryFoldersRequest(Username = username) |> DeviantArtFs.Requests.Gallery.GalleryFolders.AsyncExecute token (page 0 50)
    for f in folders.Results do
        printfn "%A %s" f.Folderid f.Name

        let! items = new DeviantArtFs.Requests.Gallery.GalleryByIdRequest(f.Folderid, Username = username) |> DeviantArtFs.Requests.Gallery.GalleryById.AsyncExecute token (page 0 3)
        for d in items.Results do
            printfn "  %s" (d.Title |> Option.defaultValue "(no title)")
            match d.CategoryPath with
            | Some s -> printfn "    Category: %s" s
            | None -> ()
            match d.PublishedTime with
            | Some s ->
                let time = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddSeconds(float s)
                printfn "    Published at: %O" time
            | None -> ()

            let! faves = DeviantArtFs.Requests.Deviation.WhoFaved.AsyncExecute token (page 0 5) d.Deviationid
            if (Seq.isEmpty faves.Results |> not) then
                printfn "    Favorited by:"
                for f in faves.Results do
                    let time = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddSeconds(float f.Time)
                    printfn "      %s at %O" f.User.Username time
                if faves.HasMore then
                    printfn "      etc."

            printfn ""

        printfn ""

    printfn "Favorites folders include:"
    printfn ""

    let! collection_folders = new DeviantArtFs.Requests.Collections.CollectionFoldersRequest(Username = username) |> DeviantArtFs.Requests.Collections.CollectionFolders.AsyncExecute token (page 0 50)
    for f in collection_folders.Results do
        printfn "%A %s" f.Folderid f.Name

        let! items = new DeviantArtFs.Requests.Collections.CollectionByIdRequest(f.Folderid, Username = username) |> DeviantArtFs.Requests.Collections.CollectionById.AsyncExecute token (page 0 3)
        for d in items.Results do
            printfn "  %s" (d.Title |> Option.defaultValue "(no title)")
            match d.CategoryPath with
            | Some s -> printfn "    Category: %s" s
            | None -> ()
            match d.PublishedTime with
            | Some s ->
                let time = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddSeconds(float s)
                printfn "    Published at: %O" time
            | None -> ()

            let! faves = DeviantArtFs.Requests.Deviation.WhoFaved.AsyncExecute token (page 0 5) d.Deviationid
            if (Seq.isEmpty faves.Results |> not) then
                printfn "    Favorited by:"
                for f in faves.Results do
                    let time = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddSeconds(float f.Time)
                    printfn "      %s at %O" f.User.Username time
                if faves.HasMore then
                    printfn "      etc."

            printfn ""

        printfn ""
}

[<EntryPoint>]
[<STAThread>]
let main _ =
    let token_string = get_token
    sandbox token_string |> Async.RunSynchronously
    0