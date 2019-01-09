This is a list of functions in the DeviantArtFs library that call DeviantArt / Sta.sh API endpoints.

Methods that return an Async<T> are intended for use from F#, and methods that return a Task<T> can be used from async methods in C# and VB.NET.

"???" indicates a type generated from a JSON sample by FSharp.Data's JsonProvider.

"long" indicates a 64-bit integer, and a question mark (?) following a type name indicates a Nullable<T>, as in C#.

**FieldChange:**

"FieldChange" is a discriminated union used in update operations. FieldChange.NoChange means the parameter will not be included; for parameters you want to include, wrap it in FieldChange.UpdateToValue, like so:

    // C#
    new DeviantArtFs.Requests.Stash.UpdateRequest(4567890123456789L) {
        Title = FieldChange<string>.NewUpdateToValue("new title"),
        Description = FieldChange<string>.NoChange
    }

> Note: Some fields can be null, and some cannot. For example, DeviantArt allows a null description for a Sta.sh stack, but not a null title.

### DeviantArtFs.DeviantArtAuth
* AsyncGetToken (string) (Uri) -> `Async<IDeviantArtRefreshToken>`
* AsyncRefresh (string) -> `Async<IDeviantArtRefreshToken>`
* GetTokenAsync (string) (Uri) -> `Task<IDeviantArtRefreshToken>`
* RefreshAsync (string) -> `Task<IDeviantArtRefreshToken>`

### DeviantArtFs.Requests.Browse.CategoryTree
* AsyncExecute (IDeviantArtAccessToken) (string) -> `Async<IEnumerable<???>>`
* ExecuteAsync (IDeviantArtAccessToken) (string) -> `Task<IEnumerable<Interop.ICategory>>`

### DeviantArtFs.Requests.Browse.DailyDeviations
* AsyncExecute (IDeviantArtAccessToken) (DailyDeviationsRequest) -> `Async<IEnumerable<???>>`
* ExecuteAsync (IDeviantArtAccessToken) (DailyDeviationsRequest) -> `Task<IEnumerable<Interop.Deviation>>`

**DailyDeviationsRequest:**

* Date: `DateTime?`

### DeviantArtFs.Requests.Browse.Hot
* AsyncExecute (IDeviantArtAccessToken) (HotRequest) -> `Async<DeviantArtPagedResult<???>>`
* ExecuteAsync (IDeviantArtAccessToken) (HotRequest) -> `Task<IDeviantArtPagedResult<Interop.Deviation>>`

**HotRequest:**

* CategoryPath: `string`
* Offset: `int`
* Limit: `int`

### DeviantArtFs.Requests.Browse.MoreLikeThis
* AsyncExecute (IDeviantArtAccessToken) (MoreLikeThisRequest) -> `Async<DeviantArtPagedResult<???>>`
* ExecuteAsync (IDeviantArtAccessToken) (MoreLikeThisRequest) -> `Task<IDeviantArtPagedResult<Interop.Deviation>>`

**MoreLikeThisRequest:**

* Seed: `Guid`
* Category: `string`
* Offset: `int`
* Limit: `int`

### DeviantArtFs.Requests.Browse.MoreLikeThisPreview
* AsyncExecute (IDeviantArtAccessToken) (Guid) -> `Async<MoreLikeThisResult<???>>`
* ExecuteAsync (IDeviantArtAccessToken) (Guid) -> `Task<MoreLikeThisResult<Interop.Deviation>>`

### DeviantArtFs.Requests.Browse.Newest
* AsyncExecute (IDeviantArtAccessToken) (NewestRequest) -> `Async<DeviantArtPagedResult<???>>`
* ExecuteAsync (IDeviantArtAccessToken) (NewestRequest) -> `Task<IDeviantArtPagedResult<Interop.Deviation>>`

**NewestRequest:**

* CategoryPath: `string`
* Q: `string`
* Offset: `int`
* Limit: `int`

### DeviantArtFs.Requests.Browse.Popular
* AsyncExecute (IDeviantArtAccessToken) (PopularRequest) -> `Async<DeviantArtPagedResult<???>>`
* ExecuteAsync (IDeviantArtAccessToken) (PopularRequest) -> `Task<IDeviantArtPagedResult<Interop.Deviation>>`

**PopularRequest:**

* CategoryPath: `string`
* Q: `string`
* Timerange: `PopularTimeRange` (EightHours, TwentyFourHours, ThreeDays, OneWeek, OneMonth, AllTime)
* Offset: `int`
* Limit: `int`

### DeviantArtFs.Requests.Browse.Tags
* AsyncExecute (IDeviantArtAccessToken) (TagsRequest) -> `Async<DeviantArtPagedResult<???>>`
* ExecuteAsync (IDeviantArtAccessToken) (TagsRequest) -> `Task<IDeviantArtPagedResult<Interop.Deviation>>`

**TagsRequest:**

* Tag: `string`
* Offset: `int`
* Limit: `int`

### DeviantArtFs.Requests.Browse.TagsSearch
* AsyncExecute (IDeviantArtAccessToken) (string) -> `Async<IEnumerable<string>>`
* ExecuteAsync (IDeviantArtAccessToken) (string) -> `Task<IEnumerable<string>>`

### DeviantArtFs.Requests.Browse.Undiscovered
* AsyncExecute (IDeviantArtAccessToken) (UndiscoveredRequest) -> `Async<DeviantArtPagedResult<???>>`
* ExecuteAsync (IDeviantArtAccessToken) (UndiscoveredRequest) -> `Task<IDeviantArtPagedResult<Interop.Deviation>>`

**UndiscoveredRequest:**

* CategoryPath: `string`
* Offset: `int`
* Limit: `int`

### DeviantArtFs.Requests.Browse.UserJournals
* AsyncExecute (IDeviantArtAccessToken) (UserJournalsRequest) -> `Async<DeviantArtPagedResult<???>>`
* ExecuteAsync (IDeviantArtAccessToken) (UserJournalsRequest) -> `Task<IDeviantArtPagedResult<Interop.Deviation>>`

**UserJournalsRequest:**

* Username: `string`
* Featured: `bool`
* Offset: `int`
* Limit: `int`

### DeviantArtFs.Requests.Collections.CollectionById
* AsyncExecute (IDeviantArtAccessToken) (CollectionByIdRequest) -> `Async<DeviantArtPagedResult<???>>`
* ExecuteAsync (IDeviantArtAccessToken) (CollectionByIdRequest) -> `Task<IDeviantArtPagedResult<Interop.Deviation>>`

**CollectionByIdRequest:**

* Folderid: `Guid`
* Username: `string`
* Offset: `int`
* Limit: `int`

### DeviantArtFs.Requests.Collections.CollectionFolders
* AsyncExecute (IDeviantArtAccessToken) (CollectionFoldersRequest) -> `Async<DeviantArtPagedResult<???>>`
* ExecuteAsync (IDeviantArtAccessToken) (CollectionFoldersRequest) -> `Task<IDeviantArtPagedResult<Interop.IDeviantArtFolder>>`

**CollectionFoldersRequest:**

* Username: `string`
* CalculateSize: `bool`
* Offset: `int`
* Limit: `int`

### DeviantArtFs.Requests.Collections.CreateCollectionFolder
* AsyncExecute (IDeviantArtAccessToken) (string) -> `Async<???>`
* ExecuteAsync (IDeviantArtAccessToken) (string) -> `Task<Interop.IDeviantArtFolder>`

### DeviantArtFs.Requests.Collections.Fave
* AsyncExecute (IDeviantArtAccessToken) (Guid) (IEnumerable<Guid>) -> `Async<int>`
* ExecuteAsync (IDeviantArtAccessToken) (Guid) (IEnumerable<Guid>) -> `Task<int>`

### DeviantArtFs.Requests.Collections.RemoveCollectionFolder
* AsyncExecute (IDeviantArtAccessToken) (Guid) -> `Async<unit>`
* ExecuteAsync (IDeviantArtAccessToken) (Guid) -> `Task`

### DeviantArtFs.Requests.Collections.Unfave
* AsyncExecute (IDeviantArtAccessToken) (Guid) (IEnumerable<Guid>) -> `Async<int>`
* ExecuteAsync (IDeviantArtAccessToken) (Guid) (IEnumerable<Guid>) -> `Task<int>`

### DeviantArtFs.Requests.Data.Countries
* AsyncExecute (IDeviantArtAccessToken) -> `Async<IEnumerable<???>>`
* ExecuteAsync (IDeviantArtAccessToken) -> `Task<IDictionary<int, string>>`

### DeviantArtFs.Requests.Data.Privacy
* AsyncExecute (IDeviantArtAccessToken) -> `Async<string>`
* ExecuteAsync (IDeviantArtAccessToken) -> `Task<string>`

### DeviantArtFs.Requests.Data.Submission
* AsyncExecute (IDeviantArtAccessToken) -> `Async<string>`
* ExecuteAsync (IDeviantArtAccessToken) -> `Task<string>`

### DeviantArtFs.Requests.Data.Tos
* AsyncExecute (IDeviantArtAccessToken) -> `Async<string>`
* ExecuteAsync (IDeviantArtAccessToken) -> `Task<string>`

### DeviantArtFs.Requests.Deviation.Content
* AsyncExecute (IDeviantArtAccessToken) (Guid) -> `Async<???>`
* ExecuteAsync (IDeviantArtAccessToken) (Guid) -> `Task<Interop.IContentResult>`

### DeviantArtFs.Requests.Deviation.DeviationById
* AsyncExecute (IDeviantArtAccessToken) (Guid) -> `Async<???>`
* ExecuteAsync (IDeviantArtAccessToken) (Guid) -> `Task<Interop.Deviation>`

### DeviantArtFs.Requests.Deviation.Download
* AsyncExecute (IDeviantArtAccessToken) (Guid) -> `Async<???>`
* ExecuteAsync (IDeviantArtAccessToken) (Guid) -> `Task<Interop.IDeviantArtFile>`

### DeviantArtFs.Requests.Deviation.EmbeddedContent
* AsyncExecute (IDeviantArtAccessToken) (EmbeddedContentRequest) -> `Async<DeviantArtPagedResult<???>>`
* ExecuteAsync (IDeviantArtAccessToken) (EmbeddedContentRequest) -> `Task<IDeviantArtPagedResult<Interop.Deviation>>`

**EmbeddedContentRequest:**

* Deviationid: `Guid`
* OffsetDeviationid: `Guid?`
* Offset: `int`
* Limit: `int`

### DeviantArtFs.Requests.Deviation.MetadataById
* AsyncExecute (IDeviantArtAccessToken) (MetadataRequest) -> `Async<IEnumerable<???>>`
* ExecuteAsync (IDeviantArtAccessToken) (MetadataRequest) -> `Task<IEnumerable<Interop.Metadata>>`

**MetadataRequest:**

* Deviationids: `IEnumerable<Guid>`
* ExtParams: `ExtParams`
* ExtCollection: `bool`

### DeviantArtFs.Requests.Deviation.WhoFaved
* AsyncExecute (IDeviantArtAccessToken) (WhoFavedRequest) -> `Async<DeviantArtPagedResult<WhoFavedUser>>`
* ExecuteAsync (IDeviantArtAccessToken) (WhoFavedRequest) -> `Task<IDeviantArtPagedResult<WhoFavedUser>>`

**WhoFavedRequest:**

* Deviationid: `Guid`
* Offset: `int`
* Limit: `int`

### DeviantArtFs.Requests.Gallery.CreateGalleryFolder
* AsyncExecute (IDeviantArtAccessToken) (string) -> `Async<???>`
* ExecuteAsync (IDeviantArtAccessToken) (string) -> `Task<Interop.IDeviantArtFolder>`

### DeviantArtFs.Requests.Gallery.GalleryAllView
* AsyncExecute (IDeviantArtAccessToken) (GalleryAllViewRequest) -> `Async<DeviantArtPagedResult<???>>`
* ExecuteAsync (IDeviantArtAccessToken) (GalleryAllViewRequest) -> `Task<IDeviantArtPagedResult<Interop.Deviation>>`

**GalleryAllViewRequest:**

* Username: `string`
* Offset: `int`
* Limit: `int`

### DeviantArtFs.Requests.Gallery.GalleryById
* AsyncExecute (IDeviantArtAccessToken) (GalleryByIdRequest) -> `Async<DeviantArtPagedResult<???>>`
* ExecuteAsync (IDeviantArtAccessToken) (GalleryByIdRequest) -> `Task<IDeviantArtPagedResult<Interop.Deviation>>`

**GalleryByIdRequest:**

* Folderid: `Guid`
* Username: `string`
* Mode: `GalleryRequestMode` (Popular, Newest)
* Offset: `int`
* Limit: `int`

### DeviantArtFs.Requests.Gallery.GalleryFolders
* AsyncExecute (IDeviantArtAccessToken) (GalleryFoldersRequest) -> `Async<DeviantArtPagedResult<???>>`
* ExecuteAsync (IDeviantArtAccessToken) (GalleryFoldersRequest) -> `Task<IDeviantArtPagedResult<Interop.IDeviantArtFolder>>`

**GalleryFoldersRequest:**

* Username: `string`
* CalculateSize: `bool`
* Offset: `int`
* Limit: `int`

### DeviantArtFs.Requests.Gallery.RemoveGalleryFolder
* AsyncExecute (IDeviantArtAccessToken) (Guid) -> `Async<unit>`
* ExecuteAsync (IDeviantArtAccessToken) (Guid) -> `Task`

### DeviantArtFs.Requests.Stash.Contents
* AsyncExecute (IDeviantArtAccessToken) (long) -> `Async<DeviantArtPagedResult<???>>`
* AsyncGetRoot (IDeviantArtAccessToken) -> `Async<DeviantArtPagedResult<???>>`
* ExecuteAsync (IDeviantArtAccessToken) (long) -> `Task<IDeviantArtPagedResult<Interop.StashMetadata>>`
* GetRootAsync (IDeviantArtAccessToken) -> `Task<IDeviantArtPagedResult<Interop.StashMetadata>>`

### DeviantArtFs.Requests.Stash.Delete
* AsyncExecute (IDeviantArtAccessToken) (long) -> `Async<unit>`
* ExecuteAsync (IDeviantArtAccessToken) (long) -> `Task`

### DeviantArtFs.Requests.Stash.Delta
* AsyncExecute (IDeviantArtAccessToken) (DeltaRequest) -> `Async<DeltaResult>`
* ExecuteAsync (IDeviantArtAccessToken) (DeltaRequest) -> `Task<Interop.IDeltaResult>`

**DeltaRequest:**

* Cursor: `string`
* Offset: `int`
* Limit: `int`
* ExtParams: `ExtParams`

### DeviantArtFs.Requests.Stash.Item
* AsyncExecute (IDeviantArtAccessToken) (ItemRequest) -> `Async<???>`
* ExecuteAsync (IDeviantArtAccessToken) (ItemRequest) -> `Task<Interop.StashMetadata>`

**ItemRequest:**

* Itemid: `long`
* ExtParams: `ExtParams`

### DeviantArtFs.Requests.Stash.Move
* AsyncExecute (IDeviantArtAccessToken) (long) (long) -> `Async<MoveResult>`
* ExecuteAsync (IDeviantArtAccessToken) (long) (long) -> `Task<Interop.IMoveResult>`

### DeviantArtFs.Requests.Stash.Position
* AsyncExecute (IDeviantArtAccessToken) (long) (int) -> `Async<unit>`
* ExecuteAsync (IDeviantArtAccessToken) (long) (int) -> `Task`

### DeviantArtFs.Requests.Stash.Publish
* AsyncExecute (IDeviantArtAccessToken) (PublishRequest) -> `Async<PublishResult>`
* ExecuteAsync (IDeviantArtAccessToken) (PublishRequest) -> `Task<PublishResult>`

**PublishRequest:**

* IsMature: `bool`
* MatureLevel: `MatureLevel` (None, Strict, Moderate)
* MatureClassification: `MatureClassification` (None, Nudity, Sexual, Gore, Language, Ideology)
* AgreeSubmission: `bool`
* AgreeTos: `bool`
* Catpath: `string`
* Feature: `bool`
* AllowComments: `bool`
* RequestCritique: `bool`
* DisplayResolution: `DisplayResolution` (Original, Max400Px, Max600px, Max800px, Max900px, Max1024px, Max1280px, Max1600px)
* Sharing: `Sharing` (Allow, HideShareButtons, HideAndMembersOnly)
* LicenseOptions: `LicenseOptions`
* Galleryids: `IEnumerable<Guid>`
* AllowFreeDownload: `bool`
* AddWatermark: `bool`
* Itemid: `long`

### DeviantArtFs.Requests.Stash.PublishCategoryTree
* AsyncExecute (IDeviantArtAccessToken) (PublishCategoryTreeRequest) -> `Async<IEnumerable<???>>`
* ExecuteAsync (IDeviantArtAccessToken) (PublishCategoryTreeRequest) -> `Task<IEnumerable<Interop.ICategory>>`

**PublishCategoryTreeRequest:**

* Catpath: `string`
* Filetype: `string`
* Frequent: `bool`

### DeviantArtFs.Requests.Stash.PublishUserdata
* AsyncExecute (IDeviantArtAccessToken) -> `Async<PublishUserdataResult>`
* ExecuteAsync (IDeviantArtAccessToken) -> `Task<PublishUserdataResult>`

### DeviantArtFs.Requests.Stash.Space
* AsyncExecute (IDeviantArtAccessToken) -> `Async<SpaceResult>`
* ExecuteAsync (IDeviantArtAccessToken) -> `Task<SpaceResult>`

### DeviantArtFs.Requests.Stash.Stack
* AsyncExecute (IDeviantArtAccessToken) (long) -> `Async<???>`
* ExecuteAsync (IDeviantArtAccessToken) (long) -> `Task<Interop.StashMetadata>`

### DeviantArtFs.Requests.Stash.Submit
* AsyncExecute (IDeviantArtAccessToken) (SubmitRequest) -> `Async<long>`
* ExecuteAsync (IDeviantArtAccessToken) (SubmitRequest) -> `Task<long>`

**SubmitRequest:**

* Filename: `string`
* ContentType: `string`
* Data: `byte[]`
* Title: `string`
* ArtistComments: `string`
* Tags: `IEnumerable<string>`
* OriginalUrl: `string`
* IsDirty: `bool?`
* Itemid: `long?`
* Stack: `string`
* Stackid: `long?`

### DeviantArtFs.Requests.Stash.Update
* AsyncExecute (IDeviantArtAccessToken) (UpdateRequest) -> `Async<unit>`
* ExecuteAsync (IDeviantArtAccessToken) (UpdateRequest) -> `Task`

**UpdateRequest:**

* Stackid: `long`
* Title: `FieldChange<string>`
* Description: `FieldChange<string>`

### DeviantArtFs.Requests.User.dAmnToken
* AsyncExecute (IDeviantArtAccessToken) -> `Async<string>`
* ExecuteAsync (IDeviantArtAccessToken) -> `Task<string>`

### DeviantArtFs.Requests.User.Friends
* AsyncExecute (IDeviantArtAccessToken) (FriendsRequest) -> `Async<DeviantArtPagedResult<FriendRecord>>`
* ExecuteAsync (IDeviantArtAccessToken) (FriendsRequest) -> `Task<IDeviantArtPagedResult<FriendRecord>>`

**FriendsRequest:**

* Username: `string`
* Offset: `int`
* Limit: `int`

### DeviantArtFs.Requests.User.FriendsSearch
* AsyncExecute (IDeviantArtAccessToken) (FriendsSearchRequest) -> `Async<IEnumerable<DeviantArtUser>>`
* ExecuteAsync (IDeviantArtAccessToken) (FriendsSearchRequest) -> `Task<IEnumerable<DeviantArtUser>>`

**FriendsSearchRequest:**

* Query: `string`
* Username: `string`

### DeviantArtFs.Requests.User.FriendsUnwatch
* AsyncExecute (IDeviantArtAccessToken) (string) -> `Async<unit>`
* ExecuteAsync (IDeviantArtAccessToken) (string) -> `Task`

### DeviantArtFs.Requests.User.FriendsWatch
* AsyncExecute (IDeviantArtAccessToken) (FriendsWatchRequest) -> `Async<unit>`
* ExecuteAsync (IDeviantArtAccessToken) (FriendsWatchRequest) -> `Task`

**FriendsWatchRequest:**

* Username: `string`
* Friend: `bool`
* Deviations: `bool`
* Journals: `bool`
* ForumThreads: `bool`
* Critiques: `bool`
* Scraps: `bool`
* Activity: `bool`
* Collections: `bool`

### DeviantArtFs.Requests.User.FriendsWatching
* AsyncExecute (IDeviantArtAccessToken) (string) -> `Async<bool>`
* ExecuteAsync (IDeviantArtAccessToken) (string) -> `Task<bool>`

### DeviantArtFs.Requests.User.ProfileByName
* AsyncExecute (IDeviantArtAccessToken) (ProfileByNameRequest) -> `Async<???>`
* ExecuteAsync (IDeviantArtAccessToken) (ProfileByNameRequest) -> `Task<Interop.Profile>`

**ProfileByNameRequest:**

* Username: `string`
* ExtCollections: `bool`
* ExtGalleries: `bool`

### DeviantArtFs.Requests.User.ProfileUpdate
* AsyncExecute (IDeviantArtAccessToken) (ProfileUpdateRequest) -> `Async<unit>`
* ExecuteAsync (IDeviantArtAccessToken) (ProfileUpdateRequest) -> `Task`

**ProfileUpdateRequest:**

* UserIsArtist: `FieldChange<bool>`
* ArtistLevel: `FieldChange<ArtistLevel>`
* ArtistSpecialty: `FieldChange<ArtistSpecialty>`
* RealName: `FieldChange<string>`
* Tagline: `FieldChange<string>`
* Countryid: `FieldChange<int>`
* Website: `FieldChange<string>`
* Bio: `FieldChange<string>`

### DeviantArtFs.Requests.User.StatusesList
* AsyncExecute (IDeviantArtAccessToken) (StatusesListRequest) -> `Async<DeviantArtPagedResult<???>>`
* ExecuteAsync (IDeviantArtAccessToken) (StatusesListRequest) -> `Task<IDeviantArtPagedResult<Interop.Status>>`

**StatusesListRequest:**

* Username: `string`
* Offset: `int`
* Limit: `int`

### DeviantArtFs.Requests.User.StatusesStatus
* AsyncExecute (IDeviantArtAccessToken) (Guid) -> `Async<???>`
* ExecuteAsync (IDeviantArtAccessToken) (Guid) -> `Task<Interop.Status>`

### DeviantArtFs.Requests.User.StatusPost
* AsyncExecute (IDeviantArtAccessToken) (StatusPostRequest) -> `Async<Guid>`
* ExecuteAsync (IDeviantArtAccessToken) (StatusPostRequest) -> `Task<Guid>`

**StatusPostRequest:**

* Body: `string`
* Statusid: `Guid?`
* Parentid: `Guid?`
* Stashid: `long?`

### DeviantArtFs.Requests.User.Watchers
* AsyncExecute (IDeviantArtAccessToken) (WatchersRequest) -> `Async<DeviantArtPagedResult<WatcherRecord>>`
* ExecuteAsync (IDeviantArtAccessToken) (WatchersRequest) -> `Task<IDeviantArtPagedResult<WatcherRecord>>`

**WatchersRequest:**

* Username: `string`
* Offset: `int`
* Limit: `int`

### DeviantArtFs.Requests.User.Whoami
* AsyncExecute (IDeviantArtAccessToken) -> `Async<DeviantArtUser>`
* ExecuteAsync (IDeviantArtAccessToken) -> `Task<DeviantArtUser>`

### DeviantArtFs.Requests.User.Whois
* AsyncExecute (IDeviantArtAccessToken) (IEnumerable<string>) -> `Async<IEnumerable<DeviantArtUser>>`
* ExecuteAsync (IDeviantArtAccessToken) (IEnumerable<string>) -> `Task<IEnumerable<DeviantArtUser>>`

### DeviantArtFs.Requests.Util.Placebo
* AsyncIsValid (IDeviantArtAccessToken) -> `Async<bool>`
* IsValidAsync (IDeviantArtAccessToken) -> `Task<bool>`

