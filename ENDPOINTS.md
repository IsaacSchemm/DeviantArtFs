This is a list of functions in the DeviantArtFs library that call DeviantArt / Sta.sh API endpoints.

Methods that return an Async<T> or AsyncSeq<T> are intended for use from F#, and their return values use option types to represent missing or null fields.

Methods that return a Task<T> can be used from async methods in C# and VB.NET, and their return values use null or Nullable<T> to represent missing or null fields.

"long" indicates a 64-bit integer, and a question mark (?) following a type name indicates a Nullable<T>, as in C#.

**IDeviantArtAccessToken**:

An interface that provides an "AccessToken" string property. You can get one from DeviantArtFs.DeviantArtAuth or implement the interface yourself.

**IExtParams:**

The value of "IExtParams" determines what extra data (if any) is included with deviations and Sta.sh metadata.

    // C#
    IExtParams e1 = new ExtParams { ExtSubmission = true, ExtCamera = false, ExtStats = false };
    IExtParams e2 = ExtParams.None;
    IExtParams e3 = ExtParams.All;

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
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (HotRequest) -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq (IDeviantArtAccessToken) (HotRequest) (int) -> `AsyncSeq<Deviation>`

**HotRequest:**

* CategoryPath: `string`

### DeviantArtFs.Requests.Browse.MoreLikeThis
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (MoreLikeThisRequest) -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (MoreLikeThisRequest) -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq (IDeviantArtAccessToken) (MoreLikeThisRequest) (int) -> `AsyncSeq<Deviation>`

**MoreLikeThisRequest:**

* Seed: `Guid`
* Category: `string`

### DeviantArtFs.Requests.Browse.MoreLikeThisPreview
* AsyncExecute (IDeviantArtAccessToken) (Guid) -> `Async<IMoreLikeThisPreviewResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (Guid) -> `Task<IMoreLikeThisPreviewResult<IBclDeviation>>`

### DeviantArtFs.Requests.Browse.Newest
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (NewestRequest) -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (NewestRequest) -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`

**NewestRequest:**

* CategoryPath: `string`
* Q: `string`

### DeviantArtFs.Requests.Browse.Popular
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (PopularRequest) -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (PopularRequest) -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`

**PopularRequest:**

* CategoryPath: `string`
* Q: `string`
* Timerange: `PopularTimeRange` (EightHours, TwentyFourHours, ThreeDays, OneWeek, OneMonth, AllTime)

### DeviantArtFs.Requests.Browse.Tags
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (string) -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (string) -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`

### DeviantArtFs.Requests.Browse.TagsSearch
* AsyncExecute (IDeviantArtAccessToken) (string) -> `Async<IEnumerable<string>>`
* ExecuteAsync (IDeviantArtAccessToken) (string) -> `Task<IEnumerable<string>>`

### DeviantArtFs.Requests.Browse.Undiscovered
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (UndiscoveredRequest) -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (UndiscoveredRequest) -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`

**UndiscoveredRequest:**

* CategoryPath: `string`

### DeviantArtFs.Requests.Browse.UserJournals
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (UserJournalsRequest) -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (UserJournalsRequest) -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq (IDeviantArtAccessToken) (UserJournalsRequest) (int) -> `AsyncSeq<Deviation>`
* ToArrayAsync (IDeviantArtAccessToken) (UserJournalsRequest) (int) (int) -> `Task<IBclDeviation[]>`

**UserJournalsRequest:**

* Username: `string`
* Featured: `bool`

### DeviantArtFs.Requests.Collections.CollectionById
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (CollectionByIdRequest) -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (CollectionByIdRequest) -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq (IDeviantArtAccessToken) (CollectionByIdRequest) (int) -> `AsyncSeq<Deviation>`
* ToArrayAsync (IDeviantArtAccessToken) (CollectionByIdRequest) (int) (int) -> `Task<IBclDeviation[]>`

**CollectionByIdRequest:**

* Folderid: `Guid`
* Username: `string`

### DeviantArtFs.Requests.Collections.CollectionFolders
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (CollectionFoldersRequest) -> `Async<DeviantArtPagedResult<DeviantArtCollectionFolder>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (CollectionFoldersRequest) -> `Task<IBclDeviantArtPagedResult<IBclDeviantArtCollectionFolder>>`
* ToAsyncSeq (IDeviantArtAccessToken) (CollectionFoldersRequest) (int) -> `AsyncSeq<DeviantArtCollectionFolder>`
* ToArrayAsync (IDeviantArtAccessToken) (CollectionFoldersRequest) (int) (int) -> `Task<IBclDeviantArtCollectionFolder[]>`

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

### DeviantArtFs.Requests.Comments.DeviationComments
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (DeviationCommentsRequest) -> `Async<DeviantArtCommentPagedResult>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (DeviationCommentsRequest) -> `Task<IBclDeviantArtCommentPagedResult>`
* ToAsyncSeq (IDeviantArtAccessToken) (DeviationCommentsRequest) (int) -> `AsyncSeq<DeviantArtComment>`
* ToArrayAsync (IDeviantArtAccessToken) (DeviationCommentsRequest) (int) (int) -> `Task<IBclDeviantArtComment[]>`

**DeviationCommentsRequest:**

* Deviationid: `Guid`
* Commentid: `Guid?`
* Maxdepth: `int`

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
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (EmbeddedContentRequest) -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq (IDeviantArtAccessToken) (EmbeddedContentRequest) (int) -> `AsyncSeq<Deviation>`
* ToArrayAsync (IDeviantArtAccessToken) (EmbeddedContentRequest) (int) (int) -> `Task<IBclDeviation[]>`

**EmbeddedContentRequest:**

* Deviationid: `Guid`
* OffsetDeviationid: `Guid?`

### DeviantArtFs.Requests.Deviation.MetadataById
* AsyncExecute (IDeviantArtAccessToken) (MetadataRequest) -> `Async<DeviationMetadataResponse>`
* ExecuteAsync (IDeviantArtAccessToken) (MetadataRequest) -> `Task<IEnumerable<IBclDeviationMetadata>>`

**MetadataRequest:**

* Deviationids: `IEnumerable<Guid>`
* ExtParams: `IExtParams`
* ExtCollection: `bool`

### DeviantArtFs.Requests.Deviation.WhoFaved
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (Guid) -> `Async<DeviantArtPagedResult<IWhoFavedUser>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (Guid) -> `Task<IBclDeviantArtPagedResult<IWhoFavedUser>>`
* ToAsyncSeq (IDeviantArtAccessToken) (Guid) (int) -> `AsyncSeq<IWhoFavedUser>`
* ToArrayAsync (IDeviantArtAccessToken) (Guid) (int) (int) -> `Task<IWhoFavedUser[]>`

### DeviantArtFs.Requests.Gallery.CreateGalleryFolder
* AsyncExecute (IDeviantArtAccessToken) (string) -> `Async<DeviantArtGalleryFolder>`
* ExecuteAsync (IDeviantArtAccessToken) (string) -> `Task<IBclDeviantArtGalleryFolder>`

### DeviantArtFs.Requests.Gallery.GalleryAllView
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (GalleryAllViewRequest) -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (GalleryAllViewRequest) -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq (IDeviantArtAccessToken) (GalleryAllViewRequest) (int) -> `AsyncSeq<Deviation>`
* ToArrayAsync (IDeviantArtAccessToken) (GalleryAllViewRequest) (int) (int) -> `Task<IBclDeviation[]>`

**GalleryAllViewRequest:**

* Username: `string`

### DeviantArtFs.Requests.Gallery.GalleryById
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (GalleryByIdRequest) -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (GalleryByIdRequest) -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq (IDeviantArtAccessToken) (GalleryByIdRequest) (int) -> `AsyncSeq<Deviation>`
* ToArrayAsync (IDeviantArtAccessToken) (GalleryByIdRequest) (int) (int) -> `Task<IBclDeviation[]>`

**GalleryByIdRequest:**

* Folderid: `Guid`
* Username: `string`
* Mode: `GalleryRequestMode` (Popular, Newest)

### DeviantArtFs.Requests.Gallery.GalleryFolders
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (GalleryFoldersRequest) -> `Async<DeviantArtPagedResult<DeviantArtGalleryFolder>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (GalleryFoldersRequest) -> `Task<IBclDeviantArtPagedResult<IBclDeviantArtGalleryFolder>>`
* ToAsyncSeq (IDeviantArtAccessToken) (GalleryFoldersRequest) (int) -> `AsyncSeq<DeviantArtGalleryFolder>`
* ToArrayAsync (IDeviantArtAccessToken) (GalleryFoldersRequest) (int) (int) -> `Task<IBclDeviantArtGalleryFolder[]>`

**GalleryFoldersRequest:**

* Username: `string`
* CalculateSize: `bool`

### DeviantArtFs.Requests.Gallery.RemoveGalleryFolder
* AsyncExecute (IDeviantArtAccessToken) (Guid) -> `Async<unit>`
* ExecuteAsync (IDeviantArtAccessToken) (Guid) -> `Task`

### DeviantArtFs.Requests.Stash.Contents
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (long) -> `Async<DeviantArtPagedResult<StashMetadata>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (long) -> `Task<IBclDeviantArtPagedResult<IBclStashMetadata>>`
* ToAsyncSeq (IDeviantArtAccessToken) (long) (int) -> `AsyncSeq<StashMetadata>`
* ToArrayAsync (IDeviantArtAccessToken) (long) (int) (int) -> `Task<IBclStashMetadata[]>`

### DeviantArtFs.Requests.Stash.Delete
* AsyncExecute (IDeviantArtAccessToken) (long) -> `Async<unit>`
* ExecuteAsync (IDeviantArtAccessToken) (long) -> `Task`

### DeviantArtFs.Requests.Stash.Delta
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (DeltaRequest) -> `Async<StashDeltaResult>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (DeltaRequest) -> `Task<IBclStashDeltaResult>`
* GetAll (IDeviantArtAccessToken) (DeltaRequest) (int) -> `AsyncSeq<StashDeltaEntry>`
* GetAllAsArrayAsync (IDeviantArtAccessToken) (IExtParams) -> `Task<IBclStashDeltaEntry[]>`

**DeltaRequest:**

* Cursor: `string`
* ExtParams: `IExtParams`

### DeviantArtFs.Requests.Stash.Item
* AsyncExecute (IDeviantArtAccessToken) (ItemRequest) -> `Async<StashMetadata>`
* ExecuteAsync (IDeviantArtAccessToken) (ItemRequest) -> `Task<IBclStashMetadata>`

**ItemRequest:**

* Itemid: `long`
* ExtParams: `IExtParams`

### DeviantArtFs.Requests.Stash.Move
* AsyncExecute (IDeviantArtAccessToken) (long) (long) -> `Async<StashMoveResult>`
* ExecuteAsync (IDeviantArtAccessToken) (long) (long) -> `Task<IBclStashMoveResult>`

### DeviantArtFs.Requests.Stash.Position
* AsyncExecute (IDeviantArtAccessToken) (long) (int) -> `Async<unit>`
* ExecuteAsync (IDeviantArtAccessToken) (long) (int) -> `Task`

### DeviantArtFs.Requests.Stash.Publish
* AsyncExecute (IDeviantArtAccessToken) (PublishRequest) -> `Async<IStashPublishResult>`
* ExecuteAsync (IDeviantArtAccessToken) (PublishRequest) -> `Task<IStashPublishResult>`

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
* AsyncExecute (IDeviantArtAccessToken) -> `Async<IStashPublishUserdataResult>`
* ExecuteAsync (IDeviantArtAccessToken) -> `Task<IStashPublishUserdataResult>`

### DeviantArtFs.Requests.Stash.Space
* AsyncExecute (IDeviantArtAccessToken) -> `Async<IStashSpaceResult>`
* ExecuteAsync (IDeviantArtAccessToken) -> `Task<IStashSpaceResult>`

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
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (FriendsRequest) -> `Async<DeviantArtPagedResult<DeviantArtFriendRecord>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (FriendsRequest) -> `Task<IBclDeviantArtPagedResult<IBclDeviantArtFriendRecord>>`
* ToAsyncSeq (IDeviantArtAccessToken) (FriendsRequest) (int) -> `AsyncSeq<DeviantArtFriendRecord>`
* ToArrayAsync (IDeviantArtAccessToken) (FriendsRequest) (int) (int) -> `Task<IBclDeviantArtFriendRecord[]>`

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

### DeviantArtFs.Requests.User.StatusById
* AsyncExecute (IDeviantArtAccessToken) (Guid) -> `Async<DeviantArtStatus>`
* ExecuteAsync (IDeviantArtAccessToken) (Guid) -> `Task<IBclDeviantArtStatus>`

### DeviantArtFs.Requests.User.StatusesList
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (string) -> `Async<DeviantArtPagedResult<DeviantArtStatus>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (string) -> `Task<IBclDeviantArtPagedResult<IBclDeviantArtStatus>>`
* ToAsyncSeq (IDeviantArtAccessToken) (string) (int) -> `AsyncSeq<DeviantArtStatus>`
* ToArrayAsync (IDeviantArtAccessToken) (string) (int) (int) -> `Task<IBclDeviantArtStatus[]>`

### DeviantArtFs.Requests.User.StatusPost
* AsyncExecute (IDeviantArtAccessToken) (StatusPostRequest) -> `Async<Guid>`
* ExecuteAsync (IDeviantArtAccessToken) (StatusPostRequest) -> `Task<Guid>`

**StatusPostRequest:**

* Body: `string`
* Statusid: `Guid?`
* Parentid: `Guid?`
* Stashid: `long?`

### DeviantArtFs.Requests.User.Watchers
* AsyncExecute (IDeviantArtAccessToken) (PagingParams) (WatchersRequest) -> `Async<DeviantArtPagedResult<DeviantArtWatcherRecord>>`
* ExecuteAsync (IDeviantArtAccessToken) (PagingParams) (WatchersRequest) -> `Task<IBclDeviantArtPagedResult<IBclDeviantArtWatcherRecord>>`
* ToAsyncSeq (IDeviantArtAccessToken) (WatchersRequest) (int) -> `AsyncSeq<DeviantArtWatcherRecord>`
* ToArrayAsync (IDeviantArtAccessToken) (WatchersRequest) (int) (int) -> `Task<IBclDeviantArtWatcherRecord[]>`

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

