namespace DeviantArtFs.Stash.Marshal

open DeviantArtFs.Stash

[<AllowNullLiteral>]
type IStashFile =
    abstract member Src: string
    abstract member Width: int
    abstract member Height: int
    abstract member Transparency: bool

module internal Utils =
    let apply (original: StackResponse.Root, modifications: StackResponse.Root) =
        let primaryJson = modifications.JsonValue.ToString().Trim()
        let primaryInner = primaryJson.Substring(1, primaryJson.Length - 2).Trim()
        let secondaryJson = original.JsonValue.ToString().Trim()
        let secondaryInner = secondaryJson.Substring(1, secondaryJson.Length - 2).Trim()
        if secondaryInner = "" then
            original
        else if primaryInner = "" then
            modifications
        else
            let parent_json = sprintf """{%s, %s}""" primaryInner secondaryInner
            StackResponse.Parse parent_json

    let toStashFile (f: StackResponse.Thumb) =
        {
            new IStashFile with
                member __.Src = f.Src
                member __.Width = f.Width
                member __.Height = f.Height
                member __.Transparency = f.Transparency
        }