namespace DeviantArtFs.Requests.Gallery

open System
open DeviantArtFs
open DeviantArtFs.Interop
open FSharp.Data

type internal FoldersElement = JsonProvider<"""[
    {
        "folderid": "47D47436-5683-8DF2-EEBF-2A6760BE1336",
        "parent": null,
        "name": "Featured",
        "size": 2
    },
    {
        "folderid": "E431BAFB-7A00-7EA1-EED7-2EF9FA0F04CE",
        "parent": "47D47436-5683-8DF2-EEBF-2A6760BE1336",
        "name": "My New Gallery"
    }
]""", SampleIsList=true>

type FoldersRequest() =
    member val Username = null with get, set
    member val CalculateSize = false with get, set
    member val Offset = 0 with get, set
    member val Limit = 10 with get, set

module Folders =
    let AsyncExecute token (ps: FoldersRequest) = async {
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
        return dafs.parseGenericList FoldersElement.Parse json
    }

    let ExecuteAsync token ps = Async.StartAsTask (async {
        let! o = AsyncExecute token ps
        return {
            HasMore = o.HasMore
            NextOffset = o.NextOffset
            Results = seq {
                for f in o.Results do
                    yield {
                        new IDeviantArtFolder with
                            member __.Folderid = f.Folderid
                            member __.Parent = f.Parent |> Option.toNullable
                            member __.Name = f.Name
                            member __.Size = f.Size |> Option.toNullable
                    }
            }
        }
    })