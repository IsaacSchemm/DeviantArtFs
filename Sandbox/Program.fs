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

    let valid = create_token_obj token_string |> DeviantArtFs.Util.Placebo.AsyncIsValid |> Async.RunSynchronously
    if valid then
        token_string
    else
        printf "Please enter the client ID (e.g. 1234)"
        let client_id = System.Console.ReadLine() |> Int32.Parse

        printf "Please enter the redirect URL (default: https://www.example.com)"
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

let sandbox token_string = async {
    printf "Enter a username (leave blank to see your own submissions):"
    let read = Console.ReadLine()

    let token = create_token_obj token_string
    let! me = DeviantArtFs.User.Whoami.AsyncExecute token

    let username =
        match read with
        | "" -> me.Username
        | s -> s

    printfn "Most recent submissions:"
    printfn ""

    let! gallery =
        new DeviantArtFs.Gallery.AllRequest(Username = username, Offset = 0, Limit = 5)
        |> DeviantArtFs.Gallery.All.AsyncExecute token
    for d in gallery.Results do
        printfn "  %s" (d.Title |> Option.defaultValue "(no title)")
        match d.CategoryPath with
        | Some s -> printfn "    Category: %s" s
        | None -> ()
        match d.PublishedTime with
        | Some s ->
            let time = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddSeconds(float s)
            printfn "    Published at: %O" time
        | None -> ()

        let! faves = new DeviantArtFs.Deviation.WhoFavedRequest(d.Deviationid) |> DeviantArtFs.Deviation.WhoFaved.AsyncExecute token
        if (Seq.isEmpty faves.Results |> not) then
            printfn "    Favorited by:"
            for f in faves.Results do
                let time = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddSeconds(float f.Time)
                printfn "      %s at %O" f.User.Username time
            if faves.HasMore then
                printfn "      etc."

        printfn ""

    let! statuses =
        new DeviantArtFs.User.StatusesRequest(username, Offset = 0, Limit = 1)
        |> DeviantArtFs.User.Statuses.AsyncExecute token
    let status = Seq.tryHead statuses.Results
    match status with
    | Some s -> 
        printfn "Most recent status: %s (%O)" s.Body s.Ts
        printfn ""
    | None -> ()
}

[<EntryPoint>]
[<STAThread>]
let main _ =
    let token_string = get_token
    sandbox token_string |> Async.RunSynchronously
    0