namespace DeviantArtFs.Requests.Gallery

open System
open DeviantArtFs
open FSharp.Data

type internal GalleryResponse = JsonProvider<"""[
{
    "has_more": true,
    "next_offset": 2,
    "name": "string",
    "results": []
}, {
    "has_more": false,
    "next_offset": null,
    "results": []
}
]""", SampleIsList=true>

type GalleryRequestMode = Popular=1 | Newest=2

type GalleryRequest(folderid: Guid) =
    member __.Folderid = folderid
    member val Username = null with get, set
    member val Mode = GalleryRequestMode.Popular with get, set
    member val Offset = 0 with get, set
    member val Limit = 10 with get, set

type GalleryResult<'a> = {
    HasMore: bool
    NextOffset: int option
    Name: string option
    Results: seq<'a>
} with
    member this.GetNextOffset() = this.NextOffset |> Option.toNullable
    member this.GetName() = this.Name |> Option.toObj

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
        let o = GalleryResponse.Parse json
        return {
            HasMore = o.HasMore
            NextOffset = o.NextOffset
            Name = o.Name
            Results = seq {
                for element in o.Results do
                    let json = element.JsonValue.ToString()
                    yield DeviationResponse.Parse json
            }
        }
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask