namespace DeviantArtFs.Extensions

open System.Runtime.CompilerServices
open System.Threading.Tasks
open System.Runtime.InteropServices
open System
open System.Threading

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

[<Extension>]
module AsyncExtensions =
    [<Extension>]
    let StartAsTask (this, [<Optional; DefaultParameterValue(TaskCreationOptions.None)>]taskCreationOptions, [<Optional; DefaultParameterValue(Nullable())>]cancellationToken: Nullable<CancellationToken>) =
        Async.StartAsTask (this, taskCreationOptions, if cancellationToken.HasValue then cancellationToken.Value else CancellationToken.None)