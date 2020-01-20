namespace DeviantArtFs.Interop

open System.Runtime.CompilerServices
open DeviantArtFs

[<Extension>]
module DeviantArtUserExtensions =
    [<Extension>]
    let GetAge (this: DeviantArtUserDetails) =
        OptUtils.intDefault this.age

    [<Extension>]
    let GetSex (this: DeviantArtUserDetails) =
        OptUtils.stringDefault this.sex

    [<Extension>]
    let GetArtistLevel (this: DeviantArtUserProfile) =
        OptUtils.stringDefault this.artist_level

    [<Extension>]
    let GetArtistSpecialty (this: DeviantArtUserProfile) =
        OptUtils.stringDefault this.artist_specialty

    [<Extension>]
    let GetIsWatching (this: DeviantArtUser) =
        OptUtils.boolDefault this.is_watching

    [<Extension>]
    let GetDetails (this: DeviantArtUser) =
        OptUtils.recordDefault this.details

    [<Extension>]
    let GetGeo (this: DeviantArtUser) =
        OptUtils.recordDefault this.geo

    [<Extension>]
    let GetProfile (this: DeviantArtUser) =
        OptUtils.recordDefault this.profile

    [<Extension>]
    let GetStats (this: DeviantArtUser) =
        OptUtils.recordDefault this.stats