namespace DeviantArtFs

open System
open System.Net

type DeviantArtException(resp: WebResponse, body: DeviantArtBaseResponse, body_raw: string) =
    inherit Exception(body.error_description |> Option.defaultValue "An unknown DeviantArt error occurred.")

    member __.ResponseBody = body
    member __.ResponseBodyRaw = body_raw
    member __.StatusCode =
        match resp with
        | :? HttpWebResponse as h -> Nullable h.StatusCode
        | _ -> Nullable()

type InvalidRefreshTokenException(ex: Exception) =
    inherit Exception("The refresh token is invalid.", ex)