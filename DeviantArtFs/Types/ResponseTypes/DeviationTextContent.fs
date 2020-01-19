namespace DeviantArtFs

open FSharp.Json

type DeviationTextContent = {
    html: string option
    css: string option
    css_fonts: string list option
} with
    static member Parse json = Json.deserialize<DeviationTextContent> json
    member this.GetHtml() = OptUtils.stringDefault this.html
    member this.GetCss() = OptUtils.stringDefault this.css
    member this.GetCssFonts() = OptUtils.listDefault this.css_fonts