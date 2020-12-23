namespace DeviantArtFs.Extensions

open System.Runtime.CompilerServices

[<Extension>]
module OptionValueExtensions =
    [<Extension>]
    let OrNull this =
        Option.toNullable this

[<Extension>]
module OptionReferenceExtensions =
    [<Extension>]
    let OrNull this =
        Option.toObj this

[<Extension>]
module OptionBooleanExtensions =
    [<Extension>]
    let IsTrue this =
        match this with
        | Some true -> true
        | _ -> false

    [<Extension>]
    let IsFalse this =
        match this with
        | Some false -> true
        | _ -> false

[<Extension>]
module OptionListExtensions =
    [<Extension>]
    let OrEmpty this =
        Option.defaultValue List.empty this