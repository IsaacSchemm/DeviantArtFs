namespace DeviantArtFs.Json.Transforms

open FSharp.Json
open FSharp.Json.Transforms

type DateTimeOffsetEpochAsString() =
    let t = new DateTimeOffsetEpoch() :> ITypeTransform
    interface ITypeTransform with
        member __.targetType () = (fun _ -> typeof<string>) ()
        member __.toTargetType value = (fun (v: obj) -> t.toTargetType v |> sprintf "%O" :> obj) value
        member __.fromTargetType value = (fun (v: obj) -> sprintf "%O" v |> System.Int64.Parse |> t.fromTargetType) value