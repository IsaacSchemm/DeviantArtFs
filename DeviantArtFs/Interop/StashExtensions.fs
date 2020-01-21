namespace DeviantArtFs.Interop

open System.Runtime.CompilerServices
open DeviantArtFs

[<Extension>]
module StashExtensions =
    [<Extension>]
    let GetViews (this: StashStats) =
        OptUtils.intDefault this.views

    [<Extension>]
    let GetViewsToday (this: StashStats) =
        OptUtils.intDefault this.views_today

    [<Extension>]
    let GetDownloads (this: StashStats) =
        OptUtils.intDefault this.downloads

    [<Extension>]
    let GetDownloadsToday (this: StashStats) =
        OptUtils.intDefault this.downloads_today

    [<Extension>]
    let GetFileSize (this: StashSubmission) =
        OptUtils.stringDefault this.file_size

    [<Extension>]
    let GetResolution (this: StashSubmission) =
        OptUtils.stringDefault this.resolution

    [<Extension>]
    let GetSubmittedWith (this: StashSubmission) =
        OptUtils.recordDefault this.submitted_with

    [<Extension>]
    let GetArtistComments (this: StashMetadata) =
        OptUtils.stringDefault this.artist_comments
    
    [<Extension>]
    let GetCamera (this: StashMetadata) =
        OptUtils.mapDefault this.camera
    
    [<Extension>]
    let GetCategory (this: StashMetadata) =
        OptUtils.stringDefault this.category
    
    [<Extension>]
    let GetCreationTime (this: StashMetadata) =
        OptUtils.timeDefault this.creation_time
    
    [<Extension>]
    let GetDescription (this: StashMetadata) =
        OptUtils.stringDefault this.description
    
    [<Extension>]
    let GetFiles (this: StashMetadata) =
        Option.defaultValue List.empty this.files
    
    [<Extension>]
    let GetItemId (this: StashMetadata) =
        OptUtils.longDefault this.itemid
    
    [<Extension>]
    let GetOriginalUrl (this: StashMetadata) =
        OptUtils.stringDefault this.original_url
    
    [<Extension>]
    let GetParentId (this: StashMetadata) =
        OptUtils.longDefault this.parentid
    
    [<Extension>]
    let GetPath (this: StashMetadata) =
        OptUtils.stringDefault this.path
    
    [<Extension>]
    let GetSize (this: StashMetadata) =
        OptUtils.intDefault this.size
    
    [<Extension>]
    let GetStackId (this: StashMetadata) =
        OptUtils.longDefault this.stackid
    
    [<Extension>]
    let GetStats (this: StashMetadata) =
        OptUtils.recordDefault this.stats
    
    [<Extension>]
    let GetSubmission (this: StashMetadata) =
        OptUtils.recordDefault this.submission
    
    [<Extension>]
    let GetTags (this: StashMetadata) =
        OptUtils.listDefault this.tags
    
    [<Extension>]
    let GetThumb (this: StashMetadata) =
        OptUtils.recordDefault this.thumb