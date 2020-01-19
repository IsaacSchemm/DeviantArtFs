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

    let valid = create_token_obj token_string |> DeviantArtFs.Requests.Util.Placebo.AsyncIsValid |> Async.RunSynchronously
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

let page offset limit = new DeviantArtPagingParams(Offset = offset, Limit = Nullable limit)

let sandbox token_string = async {
    let token = create_token_obj token_string

    printf "Enter a username (leave blank to see your own submissions): "
    let read = Console.ReadLine()
    printfn ""

    let! me = DeviantArtFs.Requests.User.Whoami.AsyncExecute token

    let username =
        match read with
        | "" -> me.username
        | s -> s

    let! profile =
        username
        |> DeviantArtFs.Requests.User.ProfileByNameRequest
        |> DeviantArtFs.Requests.User.ProfileByName.AsyncExecute token
    printfn "%s" profile.real_name
    if not (String.IsNullOrEmpty profile.tagline) then
        printfn "%s" profile.tagline
    printfn "%d deviations" profile.stats.user_deviations
    printfn ""

    let! deviations =
        DeviantArtFs.Requests.Gallery.GalleryAllViewRequest(Username = username)
        |> DeviantArtFs.Requests.Gallery.GalleryAllView.AsyncExecute token (page 0 1)
    let deviation = Seq.tryHead deviations.results
    match deviation with
    | Some s -> 
        printfn "Most recent deviation: %s (%A)" (s.title |> Option.defaultValue "???") s.published_time

        let! metadata =
            new DeviantArtFs.Requests.Deviation.MetadataRequest([s.deviationid], ExtCollection = true, ExtParams = DeviantArtExtParams.All)
            |> DeviantArtFs.Requests.Deviation.MetadataById.AsyncExecute token
        for m in metadata do
            m.tags |> Seq.map (fun t -> sprintf "#%s" t.tag_name) |> String.concat " " |> printfn "%s"

        let! favorites =
            DeviantArtFs.Requests.Deviation.WhoFaved.ToAsyncSeq token 0 s.deviationid
            |> AsyncSeq.toArrayAsync
        if (not << Seq.isEmpty) favorites then
            printfn "Favorited by:"
            for f in favorites do
                printfn "    %s (%A)" f.user.username f.time

        let! comments =
            new DeviantArtFs.Requests.Comments.DeviationCommentsRequest(s.deviationid, Maxdepth = 5)
            |> DeviantArtFs.Requests.Comments.DeviationComments.ToAsyncSeq token 0
            |> AsyncSeq.toArrayAsync
        if (not << Seq.isEmpty) comments then
            printfn "Comments:"
        for c in comments do
            printfn "    %s: %s" c.user.username c.body

        printfn ""
    | None -> ()

    let! journals =
        DeviantArtFs.Requests.Browse.UserJournalsRequest(username, Featured = false)
        |> DeviantArtFs.Requests.Browse.UserJournals.AsyncExecute token (page 0 1)
    let journal = Seq.tryHead journals.results
    match journal with
    | Some s -> 
        printfn "Most recent journal: %s (%A)" (s.title |> Option.defaultValue "???") s.published_time

        let! favorites =
            DeviantArtFs.Requests.Deviation.WhoFaved.ToAsyncSeq token 0 s.deviationid
            |> AsyncSeq.toArrayAsync
        if (not << Seq.isEmpty) favorites then
            printfn "Favorited by:"
            for f in favorites do
                printfn "%s (%A)" f.user.username f.time

        let! comments =
            new DeviantArtFs.Requests.Comments.DeviationCommentsRequest(s.deviationid, Maxdepth = 5)
            |> DeviantArtFs.Requests.Comments.DeviationComments.ToAsyncSeq token 0
            |> AsyncSeq.toArrayAsync
        if (not << Seq.isEmpty) comments then
            printfn "Comments:"
        for c in comments do
            printfn "    %s: %s" c.user.username c.body

        printfn ""
    | None -> ()

    let! statuses = DeviantArtFs.Requests.User.StatusesList.AsyncExecute token (page 0 1) username
    let status = Seq.tryHead statuses.results
    match status with
    | Some s ->
        match (s.body, s.ts) with
        | (Some body, Some ts) -> printfn "Most recent status: %s (%O)" body ts
        | _ -> ()

        match s.statusid with
        | Some statusid ->
            let! comments =
                new DeviantArtFs.Requests.Comments.StatusCommentsRequest(statusid, Maxdepth = 5)
                |> DeviantArtFs.Requests.Comments.StatusComments.ToAsyncSeq token 0
                |> AsyncSeq.toArrayAsync
            if (not << Seq.isEmpty) comments then
                printfn "Comments:"
            for c in comments do
                printfn "    %s: %s" c.user.username c.body
        | None -> ()

        printfn ""
    | None -> ()

    let! messages =
        new DeviantArtFs.Requests.Messages.MessagesFeedRequest()
        |> DeviantArtFs.Requests.Messages.MessagesFeed.ToAsyncSeq token None
        |> AsyncSeq.take 5
        |> AsyncSeq.toListAsync
    for m in messages do
        printfn "%A" m
        let originator =
            m.originator
            |> Option.map (fun u -> u.username)
            |> Option.defaultValue "???"
        let subject =
            m.GetSubjects()
            |> Seq.tryHead
        match subject with
        | None -> printfn "New message, originator %s, no subject" originator
        | Some (:? DeviantArtUser as u) -> printfn "New message, originator %s, subject is user with ID %O and name %s" originator u.userid u.username
        | Some o -> printfn "New message, originator %s, subject = %A" originator o
}

[<EntryPoint>]
[<STAThread>]
let main _ =
    let token_string = get_token
    sandbox token_string |> Async.RunSynchronously
    0