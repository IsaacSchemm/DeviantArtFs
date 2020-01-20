namespace DeviantArtFs.Interop

open System.Runtime.CompilerServices
open DeviantArtFs

[<Extension>]
module DeviantArtStatusExtensions =
    [<Extension>]
    let WhereNotDeleted (s: DeviantArtStatus seq) = seq {
        for d in s do
            if not (isNull (d :> obj)) then
                match d.ToUnion() with
                | Deleted -> ()
                | Existing e -> yield e
    }

    [<Extension>]
    let GetEmbeddedDeviations (seq: DeviantArtStatusItem seq) =
        seq |> Seq.choose (fun i -> i.deviation)

    [<Extension>]
    let GetEmbeddedStatuses (seq: DeviantArtStatusItem seq) =
        seq |> Seq.choose (fun i -> i.status)