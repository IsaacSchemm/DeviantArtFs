This is a list of functions in the DeviantArtFs library that call DeviantArt / Sta.sh API endpoints.

Methods that return an Async<T> or AsyncSeq<T> are intended for use from F#, and methods that return a Task<T> can be used from async methods in C# and VB.NET.

"???" indicates a type generated from a JSON sample by FSharp.Data's JsonProvider.

"long" indicates a 64-bit integer, and a question mark (?) following a type name indicates a Nullable<T>, as in C#.

**IDeviantArtAccessToken**:

An interface that provides an "AccessToken" string property. You can get one from DeviantArtFs.DeviantArtAuth or implement the interface yourself.

**ExtParams:**

The value of "ExtParams" determines what extra data (if any) is included with deviations and Sta.sh metadata.

    // C#
    ExtParams e1 = new ExtParams { ExtSubmission = true, ExtCamera = false, ExtStats = false };

**FieldChange:**

"FieldChange" is a discriminated union used in update operations. FieldChange.NoChange means the parameter will not be included; for parameters you want to include, wrap it in FieldChange.UpdateToValue, like so:

    // C#
    new DeviantArtFs.Requests.Stash.UpdateRequest(4567890123456789L) {
        Title = FieldChange<string>.NewUpdateToValue("new title"),
        Description = FieldChange<string>.NoChange
    }

> Note: Some fields can be null, and some cannot. For example, DeviantArt allows a null description for a Sta.sh stack, but not a null title.

**PagingParams:**

"PagingParams" is used when the common "offset" and "limit" parameters are included in a request.

    // C#
    PagingParams x1 = new PagingParams { Offset = 0, Limit = 24 };

### DeviantArtFs.DeviantArtAuth
* AsyncGetToken (string) (Uri) -> `Async<IDeviantArtRefreshToken>`
* AsyncRefresh (string) -> `Async<IDeviantArtRefreshToken>`
* GetTokenAsync (string) (Uri) -> `Task<IDeviantArtRefreshToken>`
* RefreshAsync (string) -> `Task<IDeviantArtRefreshToken>`

### DeviantArtFs.Requests.Browse.CategoryTree
* AsyncExecute (IDeviantArtAccessToken) (string) -> `Async<IEnumerable<IDeviantArtCategory>>`
* ExecuteAsync (IDeviantArtAccessToken) (string) -> `Task<IEnumerable<IDeviantArtCategory>>`

### DeviantArtFs.Requests.Browse.DailyDeviations
* AsyncExecute (IDeviantArtAccessToken) (DailyDeviationsRequest) -> `Async<IEnumerable<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (DailyDeviationsRequest) -> `Task<IEnumerable<IBclDeviation>>`

**DailyDeviationsRequest:**

* Date: `DateTime?`

### DeviantArtFs.Requests.Browse.Hot
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (HotRequest) -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (HotRequest) -> `Task<IDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq (IDeviantArtAccessToken) (HotRequest) (int) -> `AsyncSeq<Deviation>`

**HotRequest:**

* CategoryPath: `string`

### DeviantArtFs.Requests.Browse.MoreLikeThis
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (MoreLikeThisRequest) -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (MoreLikeThisRequest) -> `Task<IDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq (IDeviantArtAccessToken) (MoreLikeThisRequest) (int) -> `AsyncSeq<Deviation>`

**MoreLikeThisRequest:**

* Seed: `Guid`
* Category: `string`

### DeviantArtFs.Requests.Browse.MoreLikeThisPreview
* AsyncExecute (IDeviantArtAccessToken) (Guid) -> `Async<MoreLikeThisResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (Guid) -> `Task<MoreLikeThisResult<IBclDeviation>>`

### DeviantArtFs.Requests.Browse.Newest
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (NewestRequest) -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (NewestRequest) -> `Task<IDeviantArtPagedResult<IBclDeviation>>`

**NewestRequest:**

* CategoryPath: `string`
* Q: `string`

### DeviantArtFs.Requests.Browse.Popular
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (PopularRequest) -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (PopularRequest) -> `Task<IDeviantArtPagedResult<IBclDeviation>>`

**PopularRequest:**

* CategoryPath: `string`
* Q: `string`
* Timerange: `PopularTimeRange` (EightHours, TwentyFourHours, ThreeDays, OneWeek, OneMonth, AllTime)

### DeviantArtFs.Requests.Browse.Tags
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (string) -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (string) -> `Task<IDeviantArtPagedResult<IBclDeviation>>`

### DeviantArtFs.Requests.Browse.TagsSearch
* AsyncExecute (IDeviantArtAccessToken) (string) -> `Async<IEnumerable<string>>`
* ExecuteAsync (IDeviantArtAccessToken) (string) -> `Task<IEnumerable<string>>`

### DeviantArtFs.Requests.Browse.Undiscovered
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (UndiscoveredRequest) -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (UndiscoveredRequest) -> `Task<IDeviantArtPagedResult<IBclDeviation>>`

**UndiscoveredRequest:**

* CategoryPath: `string`

### DeviantArtFs.Requests.Browse.UserJournals
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (UserJournalsRequest) -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (UserJournalsRequest) -> `Task<IDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq (IDeviantArtAccessToken) (UserJournalsRequest) (int) -> `AsyncSeq<Deviation>`
* ToListAsync (IDeviantArtAccessToken) (UserJournalsRequest) (int) (int) -> `Task<IEnumerable<IBclDeviation>>`

**UserJournalsRequest:**

* Username: `string`
* Featured: `bool`

### DeviantArtFs.Requests.Collections.CollectionById
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (CollectionByIdRequest) -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (CollectionByIdRequest) -> `Task<IDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq (IDeviantArtAccessToken) (CollectionByIdRequest) (int) -> `AsyncSeq<Deviation>`
* ToListAsync (IDeviantArtAccessToken) (CollectionByIdRequest) (int) (int) -> `Task<IEnumerable<IBclDeviation>>`

**CollectionByIdRequest:**

* Folderid: `Guid`
* Username: `string`

### DeviantArtFs.Requests.Collections.CollectionFolders
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (CollectionFoldersRequest) -> `Async<DeviantArtPagedResult<DeviantArtCollectionFolder>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (CollectionFoldersRequest) -> `Task<IDeviantArtPagedResult<IBclDeviantArtCollectionFolder>>`
* ToAsyncSeq (IDeviantArtAccessToken) (CollectionFoldersRequest) (int) -> `AsyncSeq<DeviantArtCollectionFolder>`
* ToListAsync (IDeviantArtAccessToken) (CollectionFoldersRequest) (int) (int) -> `Task<IEnumerable<IBclDeviantArtCollectionFolder>>`

**CollectionFoldersRequest:**

* Username: `string`
* CalculateSize: `bool`

### DeviantArtFs.Requests.Collections.CreateCollectionFolder
* AsyncExecute (IDeviantArtAccessToken) (string) -> `Async<DeviantArtCollectionFolder>`
* ExecuteAsync (IDeviantArtAccessToken) (string) -> `Task<IBclDeviantArtCollectionFolder>`

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
* AsyncExecute (IDeviantArtAccessToken) -> `Async<IDictionary<int, string>>`
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
* AsyncExecute (IDeviantArtAccessToken) (Guid) -> `Async<DeviationTextContent>`
* ExecuteAsync (IDeviantArtAccessToken) (Guid) -> `Task<IBclDeviationTextContent>`

### DeviantArtFs.Requests.Deviation.DeviationById
* AsyncExecute (IDeviantArtAccessToken) (Guid) -> `Async<Deviation>`
* ExecuteAsync (IDeviantArtAccessToken) (Guid) -> `Task<IBclDeviation>`

### DeviantArtFs.Requests.Deviation.Download
* AsyncExecute (IDeviantArtAccessToken) (Guid) -> `Async<IDeviationDownload>`
* ExecuteAsync (IDeviantArtAccessToken) (Guid) -> `Task<IDeviationDownload>`

### DeviantArtFs.Requests.Deviation.EmbeddedContent
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (EmbeddedContentRequest) -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (EmbeddedContentRequest) -> `Task<IDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq (IDeviantArtAccessToken) (EmbeddedContentRequest) (int) -> `AsyncSeq<Deviation>`
* ToListAsync (IDeviantArtAccessToken) (EmbeddedContentRequest) (int) (int) -> `Task<IEnumerable<IBclDeviation>>`

**EmbeddedContentRequest:**

* Deviationid: `Guid`
* OffsetDeviationid: `Guid?`

### DeviantArtFs.Requests.Deviation.MetadataById
* AsyncExecute (IDeviantArtAccessToken) (MetadataRequest) -> `Async<IEnumerable<DeviationMetadata>>`
* ExecuteAsync (IDeviantArtAccessToken) (MetadataRequest) -> `Task<IEnumerable<IBclDeviationMetadata>>`

**MetadataRequest:**

* Deviationids: `IEnumerable<Guid>`
* ExtParams: `ExtParams`
* ExtCollection: `bool`

### DeviantArtFs.Requests.Deviation.WhoFaved
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (Guid) -> `Async<DeviantArtPagedResult<WhoFavedUser>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (Guid) -> `Task<IDeviantArtPagedResult<WhoFavedUser>>`
* ToAsyncSeq (IDeviantArtAccessToken) (Guid) (int) -> `AsyncSeq<WhoFavedUser>`
* ToListAsync (IDeviantArtAccessToken) (Guid) (int) (int) -> `Task<FSharpList<WhoFavedUser>>`

### DeviantArtFs.Requests.Gallery.CreateGalleryFolder
* AsyncExecute (IDeviantArtAccessToken) (string) -> `Async<DeviantArtGalleryFolder>`
* ExecuteAsync (IDeviantArtAccessToken) (string) -> `Task<IBclDeviantArtGalleryFolder>`

### DeviantArtFs.Requests.Gallery.GalleryAllView
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (GalleryAllViewRequest) -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (GalleryAllViewRequest) -> `Task<IDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq (IDeviantArtAccessToken) (GalleryAllViewRequest) (int) -> `AsyncSeq<Deviation>`
* ToListAsync (IDeviantArtAccessToken) (GalleryAllViewRequest) (int) (int) -> `Task<IEnumerable<IBclDeviation>>`

**GalleryAllViewRequest:**

* Username: `string`

### DeviantArtFs.Requests.Gallery.GalleryById
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (GalleryByIdRequest) -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (GalleryByIdRequest) -> `Task<IDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq (IDeviantArtAccessToken) (GalleryByIdRequest) (int) -> `AsyncSeq<Deviation>`
* ToListAsync (IDeviantArtAccessToken) (GalleryByIdRequest) (int) (int) -> `Task<IEnumerable<IBclDeviation>>`

**GalleryByIdRequest:**

* Folderid: `Guid`
* Username: `string`
* Mode: `GalleryRequestMode` (Popular, Newest)

### DeviantArtFs.Requests.Gallery.GalleryFolders
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (GalleryFoldersRequest) -> `Async<DeviantArtPagedResult<DeviantArtGalleryFolder>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (GalleryFoldersRequest) -> `Task<IDeviantArtPagedResult<IBclDeviantArtGalleryFolder>>`
* ToAsyncSeq (IDeviantArtAccessToken) (GalleryFoldersRequest) (int) -> `AsyncSeq<DeviantArtGalleryFolder>`
* ToListAsync (IDeviantArtAccessToken) (GalleryFoldersRequest) (int) (int) -> `Task<IEnumerable<IBclDeviantArtGalleryFolder>>`

**GalleryFoldersRequest:**

* Username: `string`
* CalculateSize: `bool`

### DeviantArtFs.Requests.Gallery.RemoveGalleryFolder
* AsyncExecute (IDeviantArtAccessToken) (Guid) -> `Async<unit>`
* ExecuteAsync (IDeviantArtAccessToken) (Guid) -> `Task`

### DeviantArtFs.Requests.Stash.Contents
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (long) -> `Async<DeviantArtPagedResult<StashMetadata>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (long) -> `Task<IDeviantArtPagedResult<IBclStashMetadata>>`
* ToAsyncSeq (IDeviantArtAccessToken) (long) (int) -> `AsyncSeq<StashMetadata>`
* ToListAsync (IDeviantArtAccessToken) (long) (int) (int) -> `Task<IEnumerable<IBclStashMetadata>>`

### DeviantArtFs.Requests.Stash.Delete
* AsyncExecute (IDeviantArtAccessToken) (long) -> `Async<unit>`
* ExecuteAsync (IDeviantArtAccessToken) (long) -> `Task`

### DeviantArtFs.Requests.Stash.Delta
* AsyncExecute (IDeviantArtAccessToken) (DeltaRequest) -> `Async<StashDeltaResult>`
* ExecuteAsync (IDeviantArtAccessToken) (DeltaRequest) -> `Task<IBclStashDeltaResult>`
* GetAll (IDeviantArtAccessToken) (ExtParams) -> `AsyncSeq<StashDeltaEntry>`
* GetAllAsListAsync (IDeviantArtAccessToken) (ExtParams) -> `Task<IEnumerable<IBclStashDeltaEntry>>`

**DeltaRequest:**

* Cursor: `string`
* Offset: `int`
* Limit: `int`
* ExtParams: `ExtParams`

### DeviantArtFs.Requests.Stash.Item
* AsyncExecute (IDeviantArtAccessToken) (ItemRequest) -> `Async<StashMetadata>`
* ExecuteAsync (IDeviantArtAccessToken) (ItemRequest) -> `Task<IBclStashMetadata>`

**ItemRequest:**

* Itemid: `long`
* ExtParams: `ExtParams`

### DeviantArtFs.Requests.Stash.Move
* AsyncExecute (IDeviantArtAccessToken) (long) (long) -> `Async<StashMoveResult>`
* ExecuteAsync (IDeviantArtAccessToken) (long) (long) -> `Task<IBclStashMoveResult>`

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
* AsyncExecute (IDeviantArtAccessToken) (PublishCategoryTreeRequest) -> `Async<IEnumerable<IDeviantArtCategory>>`
* ExecuteAsync (IDeviantArtAccessToken) (PublishCategoryTreeRequest) -> `Task<IEnumerable<IDeviantArtCategory>>`

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
* AsyncExecute (IDeviantArtAccessToken) (long) -> `Async<StashMetadata>`
* ExecuteAsync (IDeviantArtAccessToken) (long) -> `Task<IBclStashMetadata>`

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
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (FriendsRequest) -> `Async<DeviantArtPagedResult<FriendRecord>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (FriendsRequest) -> `Task<IDeviantArtPagedResult<FriendRecord>>`
* ToAsyncSeq (IDeviantArtAccessToken) (FriendsRequest) (int) -> `AsyncSeq<FriendRecord>`
* ToListAsync (IDeviantArtAccessToken) (FriendsRequest) (int) (int) -> `Task<FSharpList<FriendRecord>>`

**FriendsRequest:**

* Username: `string`

### DeviantArtFs.Requests.User.FriendsSearch
* AsyncExecute (IDeviantArtAccessToken) (FriendsSearchRequest) -> `Async<IEnumerable<IDeviantArtUser>>`
* ExecuteAsync (IDeviantArtAccessToken) (FriendsSearchRequest) -> `Task<IEnumerable<IDeviantArtUser>>`

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
* AsyncExecute (IDeviantArtAccessToken) (ProfileByNameRequest) -> `Async<DeviantArtProfile>`
* ExecuteAsync (IDeviantArtAccessToken) (ProfileByNameRequest) -> `Task<IBclDeviantArtProfile>`

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
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (string) -> `Async<DeviantArtPagedResult<DeviantArtStatus>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (string) -> `Task<IDeviantArtPagedResult<IBclDeviantArtStatus>>`
* ToAsyncSeq (IDeviantArtAccessToken) (string) (int) -> `AsyncSeq<DeviantArtStatus>`
* ToListAsync (IDeviantArtAccessToken) (string) (int) (int) -> `Task<IEnumerable<IBclDeviantArtStatus>>`

### DeviantArtFs.Requests.User.StatusesStatus
* AsyncExecute (IDeviantArtAccessToken) (Guid) -> `Async<DeviantArtStatus>`
* ExecuteAsync (IDeviantArtAccessToken) (Guid) -> `Task<IBclDeviantArtStatus>`

### DeviantArtFs.Requests.User.StatusPost
* AsyncExecute (IDeviantArtAccessToken) (StatusPostRequest) -> `Async<Guid>`
* ExecuteAsync (IDeviantArtAccessToken) (StatusPostRequest) -> `Task<Guid>`

**StatusPostRequest:**

* Body: `string`
* Statusid: `Guid?`
* Parentid: `Guid?`
* Stashid: `long?`

### DeviantArtFs.Requests.User.Watchers
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (WatchersRequest) -> `Async<DeviantArtPagedResult<WatcherRecord>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (WatchersRequest) -> `Task<IDeviantArtPagedResult<WatcherRecord>>`
* ToAsyncSeq (IDeviantArtAccessToken) (WatchersRequest) (int) -> `AsyncSeq<WatcherRecord>`
* ToListAsync (IDeviantArtAccessToken) (WatchersRequest) (int) (int) -> `Task<FSharpList<WatcherRecord>>`

**WatchersRequest:**

* Username: `string`

### DeviantArtFs.Requests.User.Whoami
* AsyncExecute (IDeviantArtAccessToken) -> `Async<IDeviantArtUser>`
* ExecuteAsync (IDeviantArtAccessToken) -> `Task<IDeviantArtUser>`

### DeviantArtFs.Requests.User.Whois
* AsyncExecute (IDeviantArtAccessToken) (IEnumerable<string>) -> `Async<IEnumerable<IDeviantArtUser>>`
* ExecuteAsync (IDeviantArtAccessToken) (IEnumerable<string>) -> `Task<IEnumerable<IDeviantArtUser>>`

### DeviantArtFs.Requests.Util.Placebo
* AsyncIsValid (IDeviantArtAccessToken) -> `Async<bool>`
* IsValidAsync (IDeviantArtAccessToken) -> `Task<bool>`

