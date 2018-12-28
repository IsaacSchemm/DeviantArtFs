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
    let token = create_token_obj token_string
    do! DeviantArtFs.Stash.Position.AsyncExecute token 8955911093015225L 3
}

[<EntryPoint>]
[<STAThread>]
let main _ =
    let token_string = get_token
    sandbox token_string |> Async.RunSynchronously
    0