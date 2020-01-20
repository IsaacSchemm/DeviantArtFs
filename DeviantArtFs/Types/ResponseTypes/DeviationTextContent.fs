namespace DeviantArtFs

open FSharp.Json

type DeviationTextContent = {
    html: string option
    css: string option
    css_fonts: string list option
} with
    static member Parse json = Json.deserialize<DeviationTextContent> json