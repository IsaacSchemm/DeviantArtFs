namespace DeviantArtFs.Requests.Gallery

open DeviantArtFs
open DeviantArtFs.Interop
open FSharp.Data

type GalleryFoldersRequest() =
    member val Username = null with get, set
    member val CalculateSize = false with get, set
    member val Offset = 0 with get, set
    member val Limit = 10 with get, set

module GalleryFolders =
    let AsyncExecute token (ps: GalleryFoldersRequest) = async {
        let query = seq {
            match Option.ofObj ps.Username with
            | Some s -> yield sprintf "username=%s" (dafs.urlEncode s)
            | None -> ()
            yield sprintf "calculate_size=%b" ps.CalculateSize
            yield sprintf "offset=%d" ps.Offset
            yield sprintf "limit=%d" ps.Limit
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/folders?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return dafs.parsePage FoldersElement.Parse json
    }

    let ExecuteAsync token ps =
        AsyncExecute token ps
        |> iop.thenMapResult (fun f -> {
            new IDeviantArtFolder with
                member __.Folderid = f.Folderid
                member __.Parent = f.Parent |> Option.toNullable
                member __.Name = f.Name
                member __.Size = f.Size |> Option.toNullable
        })
        |> Async.StartAsTask