namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

type RecommendedRequest() =
    member val Q = null with get, set

module Recommended =
   let AsyncExecute token common paging (req: RecommendedRequest) = async {
       let query = seq {
           match Option.ofObj req.Q with
           | Some s -> yield sprintf "q=%s" (Dafs.urlEncode s)
           | None -> ()
           yield! QueryFor.paging paging 50
           yield! QueryFor.commonParams common
       }
       let req =
           query
           |> String.concat "&"
           |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/recommended?%s"
           |> Dafs.createRequest token
       let! json = Dafs.asyncRead req
       return DeviantArtBrowsePagedResult.Parse json
   }

   let ToAsyncSeq token common offset req =
       (fun p -> AsyncExecute token common p req)
       |> Dafs.toAsyncSeq2 offset

   let ToArrayAsync token common offset limit req =
       ToAsyncSeq token common offset req
       |> AsyncSeq.take limit
       |> AsyncSeq.toArrayAsync
       |> Async.StartAsTask

   let ExecuteAsync token common paging req =
       AsyncExecute token common paging req
       |> Async.StartAsTask