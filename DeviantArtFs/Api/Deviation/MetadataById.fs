namespace DeviantArtFs.Api.Deviation

open DeviantArtFs
open System

type MetadataRequest(deviationids: seq<Guid>) =
    member __.Deviationids = deviationids
    member val ExtParams = DeviantArtExtParams.None with get, set
    member val ExtCollection = false with get, set

module MetadataById =
    let AsyncExecute token (req: MetadataRequest) = async {
        let query = seq {
            yield! QueryFor.extParams req.ExtParams
            yield sprintf "ext_collection=%b" req.ExtCollection
            yield req.Deviationids
                |> Seq.map (fun o -> o.ToString())
                |> String.concat ","
                |> sprintf "deviationids[]=%s"
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/metadata?%s"
            |> Dafs.createRequest token DeviantArtCommonParams.Default
        let! json = Dafs.asyncRead req
        return DeviationMetadataResponse.ParseList json
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask