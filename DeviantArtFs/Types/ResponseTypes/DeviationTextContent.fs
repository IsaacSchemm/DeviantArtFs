namespace DeviantArtFs

open FSharp.Json

type IBclDeviationTextContent =
    abstract member Html: string
    abstract member Css: string
    abstract member CssFonts: seq<string>

type DeviationTextContent = {
    html: string option
    css: string option
    css_fonts: string list option
} with
    static member Parse json = Json.deserialize<DeviationTextContent> json
    interface IBclDeviationTextContent with
        member this.Html = this.html |> Option.toObj
        member this.Css = this.css |> Option.toObj
        member this.CssFonts = this.css_fonts |> Option.map Seq.ofList |> Option.defaultValue Seq.empty