namespace DeviantArtFs

open System
open System.Net.Http
open DeviantArtFs.ResponseTypes

type DeviantArtException(resp: HttpResponseMessage, body: BaseResponse, body_raw: string) =
    inherit Exception(body.error_description |> Option.defaultValue "An unknown DeviantArt error occurred.")

    member _.ResponseBody = body
    member _.ResponseBodyRaw = body_raw
    member _.StatusCode = resp.StatusCode

type InvalidRefreshTokenException(resp: HttpResponseMessage) =
    inherit Exception("The refresh token is invalid.")

    member _.Response = resp