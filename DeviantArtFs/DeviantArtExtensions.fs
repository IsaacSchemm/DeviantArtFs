namespace DeviantArtFs

open System
open System.Runtime.CompilerServices

[<Extension>]
module DeviantArtExtensions =
    [<Extension>]
    let DefaultValue (this: 'a option, that: 'a) =
        Option.defaultValue that this

    [<Extension>]
    let DefaultWith (this: 'a option, that: 'a Func) =
        Option.defaultWith (fun () -> that.Invoke()) this

    [<Extension>]
    let IsTrue this =
        Option.defaultValue false this

    [<Extension>]
    let OrEmptyList this =
        Option.defaultValue List.empty this

    [<Extension>]
    let WhereNotDeleted this = seq {
        for i in this do
            let x = i :> IDeviantArtDeletable
            if not (isNull x) && not x.IsDeleted then
                yield i
    }

[<Extension>]
module DeviantArtReferenceExtensions =
    [<Extension>]
    let OrNull this =
        Option.toObj this

[<Extension>]
module DeviantArtValueExtensions =
    [<Extension>]
    let OrNull this =
        Option.toNullable this