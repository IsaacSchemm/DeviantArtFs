namespace DeviantArtFs

open FSharp.Json

type SuccessOrErrorResponse = {
    Success: bool
    ErrorDescription: string option
} with
    static member Parse json = Json.deserialize<SuccessOrErrorResponse> json