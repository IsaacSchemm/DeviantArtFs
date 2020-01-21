namespace DeviantArtFs.Interop

open System.Runtime.CompilerServices
open DeviantArtFs

[<Extension>]
module DeviantArtProfileExtensions =
    [<Extension>]
    let GetArtistLevel (this: DeviantArtProfile) =
        OptUtils.stringDefault this.artist_level
        
    [<Extension>]
    let GetArtistSpecialty (this: DeviantArtProfile) =
        OptUtils.stringDefault this.artist_specialty
        
    [<Extension>]
    let CoverPhoto (this: DeviantArtProfile) =
        OptUtils.stringDefault this.cover_photo
        
    [<Extension>]
    let GetProfilePic (this: DeviantArtProfile) =
        OptUtils.recordDefault this.profile_pic
        
    [<Extension>]
    let GetLastStatus (this: DeviantArtProfile) =
        OptUtils.recordDefault this.last_status
        
    [<Extension>]
    let GetCollections (this: DeviantArtProfile) =
        OptUtils.listDefault this.collections
        
    [<Extension>]
    let GetGalleries (this: DeviantArtProfile) =
        OptUtils.listDefault this.galleries