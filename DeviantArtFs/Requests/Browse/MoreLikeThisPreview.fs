namespace DeviantArtFs.Requests.Browse

open System
open DeviantArtFs
open FSharp.Data

type internal MoreLikeThisResponse = JsonProvider<"""{
    "seed": "C0801604-7894-532E-BC8F-C4EE47273E6D",
    "author": {},
    "more_from_artist": [],
    "more_from_da": []
}""">

module MoreLikeThisPreview =
    let AsyncExecute token (seed: Guid) = async {
        let query = seq {
            yield sprintf "seed=%O" seed
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/morelikethis/preview?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let o = MoreLikeThisResponse.Parse json
        return {
            new IMoreLikeThisPreviewResult<Deviation> with
                member __.Seed = o.Seed
                member __.Author = o.Author.JsonValue.ToString() |> dafs.parseUser
                member __.MoreFromArtist = seq {
                    for element in o.MoreFromArtist do
                        let json = element.JsonValue.ToString()
                        yield json |> DeviationResponse.Parse |> Deviation
                }
                member __.MoreFromDa = seq {
                    for element in o.MoreFromDa do
                        let json = element.JsonValue.ToString()
                        yield json |> DeviationResponse.Parse |> Deviation
                }
        }
    }

    let ExecuteAsync token seed = Async.StartAsTask (async {
        let! o = AsyncExecute token seed
        return {
            new IMoreLikeThisPreviewResult<IBclDeviation> with
                member __.Seed = o.Seed
                member __.Author = o.Author
                member __.MoreFromArtist = o.MoreFromArtist |> Seq.map dafs.asBclDeviation
                member __.MoreFromDa = o.MoreFromDa |> Seq.map dafs.asBclDeviation
        }
    })