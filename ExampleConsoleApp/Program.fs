open System
open FSharp.Control
open DeviantArtFs

let create_token_obj str = { new IDeviantArtAccessToken with member __.AccessToken = str }

let get_token =
    printf "Please enter a DeviantArt access token: "
    Console.ReadLine()

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

    let! first_deviation =
        DeviantArtFs.Api.Gallery.GalleryAllViewRequest(Username = username)
        |> DeviantArtFs.Api.Gallery.GalleryAllView.ToAsyncSeq token DeviantArtCommonParams.Default 0
        |> AsyncSeq.filter (fun d -> not d.is_deleted)
        |> AsyncSeq.take 1
        |> AsyncSeq.tryFirst
    match first_deviation with
    | None -> ()
    | Some s -> 
        printfn "Most recent deviation: %s" (s.title |> Option.defaultValue "???")
        match s.is_downloadable with
        | Some true -> printfn "Downloadable (size = %d)" (s.download_filesize |> Option.defaultValue -1)
        | _ -> printfn "Not downloadable"

        let! metadata =
            new DeviantArtFs.Api.Deviation.MetadataRequest([s.deviationid], ExtCollection = true, ExtParams = DeviantArtExtParams.All)
            |> DeviantArtFs.Api.Deviation.MetadataById.AsyncExecute token DeviantArtCommonParams.Default
        for m in metadata do
            m.tags |> Seq.map (fun t -> sprintf "#%s" t.tag_name) |> String.concat " " |> printfn "%s"

        let! all_favorites =
            DeviantArtFs.Api.Deviation.WhoFaved.ToAsyncSeq token DeviantArtCommonParams.Default 0 s.deviationid
            |> AsyncSeq.toListAsync
        match all_favorites with
        | [] ->
            printfn "No favorites"
        | _ ->
            printfn "Favorited by:"
            for f in all_favorites do
                printfn "    %s (%A)" f.user.username f.time

        printfn ""

    let! recent_deviations =
        DeviantArtFs.Api.Gallery.GalleryAllViewRequest(Username = username)
        |> DeviantArtFs.Api.Gallery.GalleryAllView.AsyncExecute token DeviantArtCommonParams.Default (page 1 9)
    printfn "Deviations 2-10:"
    for d in recent_deviations.results do
        match (d.title, d.published_time) with
        | Some title, Some published_time ->
            printfn "%s (%s)" title (published_time.Date.ToLongDateString())
        | _ ->
            printfn "Unknown title or published_time"

    printfn ""

    let! old_deviations =
        DeviantArtFs.Api.Gallery.GalleryAllViewRequest(Username = username)
        |> DeviantArtFs.Api.Gallery.GalleryAllView.AsyncExecute token DeviantArtCommonParams.Default (page 100 5)
    printfn "Deviations 100-105:"
    for d in old_deviations.results do
        match (d.title, d.published_time) with
        | Some title, Some published_time ->
            printfn "%s (%s)" title (published_time.Date.ToLongDateString())
        | _ ->
            printfn "Unknown title or published_time"

    printfn ""
}

[<EntryPoint>]
let main _ =
    let token_string = get_token
    sandbox token_string |> Async.RunSynchronously
    0