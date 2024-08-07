﻿open System
open FSharp.Control
open DeviantArtFs
open DeviantArtFs.ParameterTypes

let create_token_obj str = {
    new IDeviantArtAccessTokenWithOptionalParameters with
        member _.AccessToken = str
        member _.OptionalParameters = [
            OptionalParameter.ExtParam ExtParam.Collection
            OptionalParameter.MatureContent true
        ]
}

let get_token =
    printf "Please enter a DeviantArt access token: "
    Console.ReadLine()

let rec print_all_comments token subject prefix replyType = async {
    let! comments =
        DeviantArtFs.Api.Comments.GetCommentsAsync token (DeviantArtFs.Api.Comments.Depth 0) subject replyType MaximumPagingLimit StartingOffset
        |> AsyncSeq.ofAsyncEnum
        |> AsyncSeq.truncate 10
        |> AsyncSeq.toListAsync
    for c in comments do
        let str = sprintf "%s%s" prefix c.body
        if str.Length > 119 then
            printfn "%s..." (str.Substring(0, 116))
        else
            printfn "%s" str
        do! print_all_comments token subject (sprintf "  %s" prefix) (DeviantArtFs.Api.Comments.InReplyToComment c.commentid)
}

let sandbox token_string = async {
    let token = create_token_obj token_string

    printf "Enter a username (leave blank to see your own submissions): "
    let read = Console.ReadLine()
    printfn ""

    let! me = DeviantArtFs.Api.User.WhoamiAsync token |> Async.AwaitTask

    let username =
        match read with
        | "" -> me.username
        | s -> s

    let! profile = DeviantArtFs.Api.User.GetProfileAsync token DeviantArtFs.Api.User.ProfileExtParams.None (UserScope.ForUser username) |> Async.AwaitTask
    printfn "%s" profile.real_name
    if not (String.IsNullOrEmpty profile.tagline) then
        printfn "%s" profile.tagline
    printfn "%d deviations" profile.stats.user_deviations

    printfn ""

    do! print_all_comments token (Api.Comments.OnProfile username) "" Api.Comments.DirectReply
    printfn ""

    let! first_deviation =
        DeviantArtFs.Api.Gallery.GetAllViewAsync
            token
            (ForUser username)
            DefaultPagingLimit
            StartingOffset
        |> AsyncSeq.ofAsyncEnum
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

        let! metadata_response = DeviantArtFs.Api.Deviation.GetMetadataAsync token [s.deviationid] |> Async.AwaitTask
        for m in metadata_response.metadata do
            m.tags |> Seq.map (fun t -> sprintf "#%s" t.tag_name) |> String.concat " " |> printfn "%s"

        let! all_favorites =
            DeviantArtFs.Api.Deviation.GetWhoFavedAsync token s.deviationid MaximumPagingLimit StartingOffset
            |> AsyncSeq.ofAsyncEnum
            |> AsyncSeq.toListAsync
        match all_favorites with
        | [] ->
            printfn "No favorites"
        | _ ->
            printfn "Favorited by:"
            for f in all_favorites do
                printfn "    %s (%A)" f.user.username f.time

        printfn ""

        do! print_all_comments token (Api.Comments.OnDeviation s.deviationid) "  " Api.Comments.DirectReply
        printfn ""

    let! recent_deviations =
        DeviantArtFs.Api.Gallery.PageAllViewAsync
            token
            (ForUser username)
            (PagingLimit 9)
            (PagingOffset 1)
        |> Async.AwaitTask
    printfn "Deviations 2-10:"
    for d in recent_deviations.results |> Option.defaultValue List.empty do
        match (d.title, d.published_time) with
        | Some title, Some published_time ->
            printfn "%s (%s)" title (published_time.Date.ToLongDateString())
        | _ ->
            printfn "Unknown title or published_time"

    printfn ""

    let! old_deviations =
        DeviantArtFs.Api.Gallery.PageAllViewAsync
            token
            (ForUser username)
            (PagingLimit 5)
            (PagingOffset 100)
        |> Async.AwaitTask
    printfn "Deviations 100-105:"
    for d in old_deviations.results |> Option.defaultValue List.empty do
        match (d.title, d.published_time) with
        | Some title, Some published_time ->
            printfn "%s (%s)" title (published_time.Date.ToLongDateString())
        | _ ->
            printfn "Unknown title or published_time"
}

[<EntryPoint>]
let main _ =
    let token_string = get_token
    sandbox token_string |> Async.RunSynchronously
    0