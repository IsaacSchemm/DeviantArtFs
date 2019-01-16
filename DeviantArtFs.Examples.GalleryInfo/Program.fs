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
        | "" -> me.username
        | s -> s

    let! deviations =
        DeviantArtFs.Requests.Gallery.GalleryAllViewRequest(Username = username)
        |> DeviantArtFs.Requests.Gallery.GalleryAllView.AsyncExecute token (page 0 1)
    let deviation = Seq.tryHead deviations.Results
    match deviation with
    | Some s -> 
        printfn "Most recent deviation: %s (%A)" (s.title |> Option.defaultValue "???") s.published_time

        let! metadata =
            new DeviantArtFs.Requests.Deviation.MetadataRequest([s.deviationid], ExtCollection = true, ExtParams = ExtParams.All)
            |> DeviantArtFs.Requests.Deviation.MetadataById.AsyncExecute token
        for m in metadata do
            printfn "%A" m

        let! favorites =
            DeviantArtFs.Requests.Deviation.WhoFaved.ToAsyncSeq token s.deviationid 0
            |> AsyncSeq.toArrayAsync
        if (not << Seq.isEmpty) favorites then
            printfn "Favorited by:"
            for f in favorites do
                printfn "    %s (%A)" f.user.username f.time

        let comments_req = new DeviantArtFs.Requests.Comments.DeviationCommentsRequest(s.deviationid, Maxdepth = 5)
        let! comments = DeviantArtFs.Requests.Comments.DeviationComments.ToAsyncSeq token comments_req 0 |> AsyncSeq.toArrayAsync
        if (not << Seq.isEmpty) comments then
            printfn "Comments:"
        for c in comments do
            printfn "    %s: %s" c.User.Username c.Body

        printfn ""
    | None -> ()

    let! journals =
        DeviantArtFs.Requests.Browse.UserJournalsRequest(username, Featured = false)
        |> DeviantArtFs.Requests.Browse.UserJournals.AsyncExecute token (page 0 1)
    let journal = Seq.tryHead journals.Results
    match journal with
    | Some s -> 
        printfn "Most recent journal: %s (%A)" (s.title |> Option.defaultValue "???") s.published_time

        let! favorites =
            DeviantArtFs.Requests.Deviation.WhoFaved.ToAsyncSeq token s.deviationid 0
            |> AsyncSeq.toArrayAsync
        if (not << Seq.isEmpty) favorites then
            printfn "Favorited by:"
            for f in favorites do
                printfn "%s (%A)" f.user.username f.time
        printfn ""
    | None -> ()

    let! statuses = DeviantArtFs.Requests.User.StatusesList.AsyncExecute token (page 0 1) username
    let status = Seq.tryHead statuses.Results
    match status with
    | Some s -> 
        printfn "Most recent status: %s (%A)" s.body s.ts
        printfn ""
    | None -> ()

    return ()
}

[<EntryPoint>]
[<STAThread>]
let main _ =
    let token_string = get_token
    sandbox token_string |> Async.RunSynchronously
    0