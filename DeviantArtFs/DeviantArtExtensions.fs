namespace DeviantArtFs

open System.Runtime.CompilerServices

[<Extension>]
module DeviantArtExtensions =
    [<Extension>]
    let WhereNotDeleted (this: 'a seq) = seq {
        for i in this do
            let x = i :> IDeviantArtDeletable
            if not (isNull x) && not x.IsDeleted then
                yield i
    }