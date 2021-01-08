namespace DeviantArtFs.Extensions

open System.Runtime.CompilerServices
open FSharp.Control
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
    let StartAsTask (this, [<Optional; DefaultParameterValue(TaskCreationOptions.None)>]taskCreationOptions, [<Optional; DefaultParameterValue(Nullable())>]cancellationToken) =
        Async.StartAsTask (this, taskCreationOptions, cancellationToken |> Option.ofNullable |> Option.defaultValue CancellationToken.None)

[<Extension>]
module AsyncSeqExtensions =
    [<Extension>]
    let Take this count =
        AsyncSeq.take count this

    [<Extension>]
    let ToArrayAsync (this, [<Optional; DefaultParameterValue(TaskCreationOptions.None)>]taskCreationOptions, [<Optional; DefaultParameterValue(Nullable())>]cancellationToken) =
        let x = AsyncSeq.toArrayAsync this
        Async.StartAsTask (x, taskCreationOptions, cancellationToken |> Option.ofNullable |> Option.defaultValue CancellationToken.None)

#if NET5_0
    [<Extension>]
    let ToAsyncEnumerable this =
        AsyncSeq.toAsyncEnum this
#endif