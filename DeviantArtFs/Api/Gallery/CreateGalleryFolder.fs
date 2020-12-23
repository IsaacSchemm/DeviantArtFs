﻿namespace DeviantArtFs.Api.Gallery

open System
open System.IO
open DeviantArtFs
open FSharp.Data

module CreateGalleryFolder =
    let AsyncExecute token (folder: string) = async {
        let query = seq {
            yield sprintf "folder=%s" (Dafs.urlEncode folder)
        }

        let req = Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/gallery/folders/create"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        req.RequestBodyText <- String.concat "&" query

        let! json = Dafs.asyncRead req
        return DeviantArtGalleryFolder.Parse json
    }

    let ExecuteAsync token folder =
        AsyncExecute token folder
        |> Async.StartAsTask