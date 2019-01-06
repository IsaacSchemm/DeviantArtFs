namespace DeviantArtFs.Stash.Marshal

open DeviantArtFs
open DeviantArtFs.Interop

module internal Utils =
    let toStashFile (f: StashMetadataResponse.Thumb) =
        {
            new IStashFile with
                member __.Src = f.Src
                member __.Width = f.Width
                member __.Height = f.Height
                member __.Transparency = f.Transparency
        }