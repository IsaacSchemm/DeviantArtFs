namespace DeviantArtFs.Extensions

open System
open System.Runtime.CompilerServices

[<Extension>]
module OptionExtensions =
    [<Extension>]
    let DefaultValue (this: 'a option, that: 'a) =
        Option.defaultValue that this

    [<Extension>]
    let DefaultWith (this: 'a option, that: 'a Func) =
        Option.defaultWith (fun () -> that.Invoke()) this

[<Extension>]
module OptionValueExtensions =
    [<Extension>]
    let OrNull this =
        Option.toNullable this

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
module OptionReferenceExtensions =
    [<Extension>]
    let OrNull this =
        Option.toObj this

[<Extension>]
module OptionListExtensions =
    [<Extension>]
    let OrEmpty this =
        Option.defaultValue List.empty this