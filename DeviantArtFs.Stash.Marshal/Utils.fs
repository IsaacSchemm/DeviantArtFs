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
        modifications

    let toStashFile (f: StackResponse.Thumb) =
        {
            new IStashFile with
                member __.Src = f.Src
                member __.Width = f.Width
                member __.Height = f.Height
                member __.Transparency = f.Transparency
        }