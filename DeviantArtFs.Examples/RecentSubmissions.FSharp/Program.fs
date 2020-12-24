open System
open System.IO
open DeviantArtFs
open FSharp.Control

// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

let create_token_obj str = { new IDeviantArtAccessToken with member __.AccessToken = str }

let get_token =
    let token_file = "token.txt"
    let token_string =
        if File.Exists token_file
        then File.ReadAllText token_file
        else ""

    let valid = create_token_obj token_string |> DeviantArtFs.Api.Util.Placebo.AsyncIsValid |> Async.RunSynchronously
    if valid then
        token_string
    else
        printf "Please enter the client ID (e.g. 1234): "
        let client_id = Console.ReadLine() |> Int32.Parse

        printf "Please enter the redirect URL (default: https://www.example.com): "
        let url1 = Console.ReadLine()
        let url2 =
            match url1 with
            | "" -> "https://www.example.com"
            | s -> s

        use form = new DeviantArtFs.WinForms.DeviantArtImplicitGrantForm(client_id, new Uri(url2), ["browse"; "user"; "stash"; "publish"; "user.manage"])
        if form.ShowDialog() <> System.Windows.Forms.DialogResult.OK then
            failwithf "Login cancelled"
        else
            File.WriteAllText(token_file, form.AccessToken)
            form.AccessToken

let page offset limit = { Offset = offset; Limit = Nullable limit }

let sandbox token_string = async {
    let token = create_token_obj token_string

    printf "Enter a username (leave blank to see your own submissions): "
    let read = Console.ReadLine()
    printfn ""

    let! me = DeviantArtFs.Api.User.Whoami.AsyncExecute token

    let username =
        match read with
        | "" -> me.username
        | s -> s

    let! profile =
        username
        |> DeviantArtFs.Api.User.ProfileByNameRequest
        |> DeviantArtFs.Api.User.ProfileByName.AsyncExecute token
    printfn "%s" profile.real_name
    if not (String.IsNullOrEmpty profile.tagline) then
        printfn "%s" profile.tagline
    printfn "%d deviations" profile.stats.user_deviations
    printfn ""

    let! deviations =
        DeviantArtFs.Api.Gallery.GalleryAllViewRequest(Username = username)
        |> DeviantArtFs.Api.Gallery.GalleryAllView.AsyncExecute token (page 0 1)
    let deviation = deviations.results |> Seq.where (fun x -> not x.is_deleted) |> Seq.tryHead
    match deviation with
    | Some s -> 
        printfn "Most recent (non-deleted) deviation: %s (%A)" (s.title |> Option.defaultValue "???") s.published_time
        match s.is_downloadable with
        | Some true -> printfn "Downloadable (size = %d}" (s.download_filesize |> Option.defaultValue -1)
        | _ -> printfn "Not downloadable"

        let! metadata =
            new DeviantArtFs.Api.Deviation.MetadataRequest([s.deviationid], ExtCollection = true, ExtParams = DeviantArtExtParams.All)
            |> DeviantArtFs.Api.Deviation.MetadataById.AsyncExecute token
        for m in metadata do
            m.tags |> Seq.map (fun t -> sprintf "#%s" t.tag_name) |> String.concat " " |> printfn "%s"

        let! favorites =
            DeviantArtFs.Api.Deviation.WhoFaved.ToAsyncSeq token 0 s.deviationid
            |> AsyncSeq.toArrayAsync
        if (not << Seq.isEmpty) favorites then
            printfn "Favorited by:"
            for f in favorites do
                printfn "    %s (%A)" f.user.username f.time

        let! comments =
            new DeviantArtFs.Api.Comments.DeviationCommentsRequest(s.deviationid, Maxdepth = 5)
            |> DeviantArtFs.Api.Comments.DeviationComments.ToAsyncSeq token DeviantArtCommonParams.Default 0
            |> AsyncSeq.toArrayAsync
        if (not << Seq.isEmpty) comments then
            printfn "Comments:"
        for c in comments do
            printfn "    %s: %s" c.user.username c.body

        printfn ""
    | None -> ()

    let! journals =
        DeviantArtFs.Api.Browse.UserJournalsRequest(username, Featured = false)
        |> DeviantArtFs.Api.Browse.UserJournals.AsyncExecute token DeviantArtCommonParams.Default (page 0 1)
    let journal = journals.results |> Seq.where (fun x -> not x.is_deleted) |> Seq.tryHead
    match journal with
    | Some s -> 
        printfn "Most recent (non-deleted) journal: %s (%A)" (s.title |> Option.defaultValue "???") s.published_time

        let! favorites =
            DeviantArtFs.Api.Deviation.WhoFaved.ToAsyncSeq token 0 s.deviationid
            |> AsyncSeq.toArrayAsync
        if (not << Seq.isEmpty) favorites then
            printfn "Favorited by:"
            for f in favorites do
                printfn "%s (%A)" f.user.username f.time

        let! comments =
            new DeviantArtFs.Api.Comments.DeviationCommentsRequest(s.deviationid, Maxdepth = 5)
            |> DeviantArtFs.Api.Comments.DeviationComments.ToAsyncSeq token DeviantArtCommonParams.Default 0
            |> AsyncSeq.toArrayAsync
        if (not << Seq.isEmpty) comments then
            printfn "Comments:"
        for c in comments do
            printfn "    %s: %s" c.user.username c.body

        printfn ""
    | None -> ()

    let! statuses = DeviantArtFs.Api.User.StatusesList.AsyncExecute token (page 0 1) username
    let status = statuses.results |> Seq.where (fun x -> not x.is_deleted) |> Seq.tryHead
    match status with
    | Some s ->
        printfn "Most recent (non-deleted) status: %s (%O)" (Option.get s.body) (Option.get s.ts)

        let! comments =
            new DeviantArtFs.Api.Comments.StatusCommentsRequest(Option.get s.statusid, Maxdepth = 5)
            |> DeviantArtFs.Api.Comments.StatusComments.ToAsyncSeq token DeviantArtCommonParams.Default 0
            |> AsyncSeq.toArrayAsync
        if (not << Seq.isEmpty) comments then
            printfn "Comments:"
        for c in comments do
            printfn "    %s: %s" c.user.username c.body

        printfn ""
    | None -> ()
}

[<EntryPoint>]
[<STAThread>]
let main _ =
    let token_string = get_token
    sandbox token_string |> Async.RunSynchronously
    0