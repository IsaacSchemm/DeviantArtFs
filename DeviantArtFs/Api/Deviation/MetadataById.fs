namespace DeviantArtFs.Api.Deviation

open DeviantArtFs
open System

type MetadataRequest(deviationids: seq<Guid>) =
    member __.Deviationids = deviationids
    member val ExtParams = DeviantArtExtParams.None with get, set
    member val ExtCollection = false with get, set

module MetadataById =
    let AsyncExecute token (req: MetadataRequest) =
        seq {
            yield! QueryFor.extParams req.ExtParams
            yield sprintf "ext_collection=%b" req.ExtCollection
            for id in req.Deviationids do
                yield sprintf "deviationids[]=%O" id
        }
        |> Dafs.createRequest Dafs.Method.GET token "https://www.deviantart.com/api/v1/oauth2/deviation/metadata"
        |> Dafs.asyncRead
        |> Dafs.thenParse<DeviationMetadataResponse>

    let ExecuteAsync token req =
        AsyncExecute token req
        |> Async.StartAsTask