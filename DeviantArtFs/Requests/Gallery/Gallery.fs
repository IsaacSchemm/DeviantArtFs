namespace DeviantArtFs.Requests.Gallery

open System
open DeviantArtFs
open DeviantArtFs.Interop
open FSharp.Data

type internal GalleryListResponse = JsonProvider<"""[
{ "has_more": true, "next_offset": 2, "name": "str", "results": [] },
{ "has_more": false, "next_offset": null, "results": [] }
]""", SampleIsList=true>

type GalleryRequestMode = Popular=1 | Newest=2

type GalleryRequest(folderid: Guid) =
    member __.Folderid = folderid
    member val Username = null with get, set
    member val Mode = GalleryRequestMode.Popular with get, set
    member val Offset = 0 with get, set
    member val Limit = 10 with get, set

module Gallery =
    let AsyncExecute token (req: GalleryRequest) = async {
        let query = seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (dafs.urlEncode s)
            | None -> ()
            yield sprintf "mode=%s" (if req.Mode = GalleryRequestMode.Newest then "newest" else "popular")
            yield sprintf "offset=%d" req.Offset
            yield sprintf "limit=%d" req.Limit
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/%A?%s" req.Folderid
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let o = GalleryListResponse.Parse json
        return {
            HasMore = o.HasMore
            NextOffset = o.NextOffset
            Name = o.Name
            Results = seq {
                for element in o.Results do
                    let json = element.JsonValue.ToString()
                    yield json |> DeviationResponse.Parse
            }
        }
    }

    let ExecuteAsync token req = AsyncExecute token req |> iop.thenMapResult Deviation |> Async.StartAsTask