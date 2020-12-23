namespace DeviantArtFs.Api.Browse

open DeviantArtFs
open FSharp.Control

module PostsByDeviantsYouWatch =
   let AsyncExecute token common paging = async {
       let query = seq {
           yield! QueryFor.paging paging 50
       }
       let req =
           query
           |> String.concat "&"
           |> sprintf "https://www.deviantart.com/api/v1/oauth2/browse/posts/deviantsyouwatch?%s"
           |> Dafs.createRequest token common
       let! json = Dafs.asyncRead req
       return DeviantArtPostsPagedResult.Parse json
   }

   let ToAsyncSeq token common offset =
       (fun p -> AsyncExecute token common p)
       |> Dafs.toAsyncSeq2 offset

   let ToArrayAsync token common offset limit =
       ToAsyncSeq token common offset
       |> AsyncSeq.take limit
       |> AsyncSeq.toArrayAsync
       |> Async.StartAsTask

   let ExecuteAsync token common paging =
       AsyncExecute token common paging
       |> Async.StartAsTask