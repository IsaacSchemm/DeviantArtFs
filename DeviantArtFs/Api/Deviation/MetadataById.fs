namespace DeviantArtFs.Api.Deviation

open DeviantArtFs
open System

type MetadataRequest(deviationids: seq<Guid>) =
    member __.Deviationids = deviationids
    member val ExtParams = DeviantArtExtParams.None with get, set
    member val ExtCollection = false with get, set

module MetadataById =
    let AsyncExecute token common (req: MetadataRequest) =
        seq {
            yield! QueryFor.extParams req.ExtParams
            yield sprintf "ext_collection=%b" req.ExtCollection
            yield req.Deviationids
                |> Seq.map (fun o -> o.ToString())
                |> String.concat ","
                |> sprintf "deviationids[]=%s"
            yield! QueryFor.commonParams common
        }
        |> Dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/deviation/metadata"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviationMetadataResponse>
        |> Dafs.extractList

    let ExecuteAsync token common req =
        AsyncExecute token common req
        |> Async.StartAsTask