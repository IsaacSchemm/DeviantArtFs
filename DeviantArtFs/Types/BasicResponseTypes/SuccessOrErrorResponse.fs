namespace DeviantArtFs

open FSharp.Json

type SuccessOrErrorResponse = {
    success: bool
    error_description: string option
} with
    static member Parse json = Json.deserialize<SuccessOrErrorResponse> json