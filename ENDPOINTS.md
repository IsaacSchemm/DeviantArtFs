This is a list of functions in the DeviantArtFs library that call DeviantArt / Sta.sh API endpoints.

Methods that return an Async<T> or AsyncSeq<T> are intended for use from F#, and their return values use option types to represent missing or null fields.

Methods that return a Task<T> can be used from async methods in C# and VB.NET, and their return values use null or Nullable<T> to represent missing or null fields.

"long" indicates a 64-bit integer, and a question mark (?) following a type name indicates a Nullable<T>, as in C#.

### DeviantArtFs.DeviantArtAuth
* AsyncGetToken `string` `Uri` -> `Async<IDeviantArtRefreshTokenFull>`
* AsyncRefresh `string` -> `Async<IDeviantArtRefreshTokenFull>`
* AsyncRevoke `string` `bool` -> `Async<unit>`
* RevokeAsync `string` `bool` -> `Task`
* GetTokenAsync `string` `Uri` -> `Task<IDeviantArtRefreshTokenFull>`
* RefreshAsync `string` -> `Task<IDeviantArtRefreshTokenFull>`

### DeviantArtFs.Requests.Browse.CategoryTree
* AsyncExecute `IDeviantArtAccessToken` `string` -> `Async<IEnumerable<DeviantArtCategory>>`
* ExecuteAsync `IDeviantArtAccessToken` `string` -> `Task<IEnumerable<IBclDeviantArtCategory>>`

### DeviantArtFs.Requests.Browse.DailyDeviations
* AsyncExecute `IDeviantArtAccessToken` `DailyDeviationsRequest` -> `Async<FSharpList<Deviation>>`
* ExecuteAsync `IDeviantArtAccessToken` `DailyDeviationsRequest` -> `Task<IEnumerable<IBclDeviation>>`

**DailyDeviationsRequest:**

* Date: `DateTime?`

### DeviantArtFs.Requests.Browse.Hot
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `HotRequest` -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `HotRequest` -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `HotRequest` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `HotRequest` -> `Task<IBclDeviation[]>`

**HotRequest:**

* CategoryPath: `string`

### DeviantArtFs.Requests.Browse.MoreLikeThis
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `MoreLikeThisRequest` -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `MoreLikeThisRequest` -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `MoreLikeThisRequest` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `MoreLikeThisRequest` -> `Task<IBclDeviation[]>`

**MoreLikeThisRequest:**

* Seed: `Guid`
* Category: `string`

### DeviantArtFs.Requests.Browse.MoreLikeThisPreview
* AsyncExecute `IDeviantArtAccessToken` `Guid` -> `Async<DeviantArtMoreLikeThisPreviewResult>`
* ExecuteAsync `IDeviantArtAccessToken` `Guid` -> `Task<IBclDeviantArtMoreLikeThisPreviewResult>`

### DeviantArtFs.Requests.Browse.Newest
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `NewestRequest` -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `NewestRequest` -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `NewestRequest` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `NewestRequest` -> `Task<IBclDeviation[]>`

**NewestRequest:**

* CategoryPath: `string`
* Q: `string`

### DeviantArtFs.Requests.Browse.Popular
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `PopularRequest` -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `PopularRequest` -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `PopularRequest` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `PopularRequest` -> `Task<IBclDeviation[]>`

**PopularRequest:**

* CategoryPath: `string`
* Q: `string`
* Timerange: `PopularTimeRange` (EightHours, TwentyFourHours, ThreeDays, OneWeek, OneMonth, AllTime)

### DeviantArtFs.Requests.Browse.Tags
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `string` -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `string` -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `string` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `string` -> `Task<IBclDeviation[]>`

### DeviantArtFs.Requests.Browse.TagsSearch
* AsyncExecute `IDeviantArtAccessToken` `string` -> `Async<FSharpList<string>>`
* ExecuteAsync `IDeviantArtAccessToken` `string` -> `Task<IEnumerable<string>>`

### DeviantArtFs.Requests.Browse.Undiscovered
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `UndiscoveredRequest` -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `UndiscoveredRequest` -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `UndiscoveredRequest` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `UndiscoveredRequest` -> `Task<IBclDeviation[]>`

**UndiscoveredRequest:**

* CategoryPath: `string`

### DeviantArtFs.Requests.Browse.UserJournals
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `UserJournalsRequest` -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `UserJournalsRequest` -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `UserJournalsRequest` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `UserJournalsRequest` -> `Task<IBclDeviation[]>`

**UserJournalsRequest:**

* Username: `string`
* Featured: `bool`

### DeviantArtFs.Requests.Collections.CollectionById
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `CollectionByIdRequest` -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `CollectionByIdRequest` -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `CollectionByIdRequest` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `CollectionByIdRequest` -> `Task<IBclDeviation[]>`

**CollectionByIdRequest:**

* Folderid: `Guid`
* Username: `string`

### DeviantArtFs.Requests.Collections.CollectionFolders
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `CollectionFoldersRequest` -> `Async<DeviantArtPagedResult<DeviantArtCollectionFolder>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `CollectionFoldersRequest` -> `Task<IBclDeviantArtPagedResult<IBclDeviantArtCollectionFolder>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `CollectionFoldersRequest` -> `AsyncSeq<DeviantArtCollectionFolder>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `CollectionFoldersRequest` -> `Task<IBclDeviantArtCollectionFolder[]>`

**CollectionFoldersRequest:**

* Username: `string`
* CalculateSize: `bool`
* ExtPreload: `bool`

### DeviantArtFs.Requests.Collections.CreateCollectionFolder
* AsyncExecute `IDeviantArtAccessToken` `string` -> `Async<DeviantArtCollectionFolder>`
* ExecuteAsync `IDeviantArtAccessToken` `string` -> `Task<IBclDeviantArtCollectionFolder>`

### DeviantArtFs.Requests.Collections.Fave
* AsyncExecute `IDeviantArtAccessToken` `Guid` `IEnumerable<Guid>` -> `Async<int>`
* ExecuteAsync `IDeviantArtAccessToken` `Guid` `IEnumerable<Guid>` -> `Task<int>`

### DeviantArtFs.Requests.Collections.RemoveCollectionFolder
* AsyncExecute `IDeviantArtAccessToken` `Guid` -> `Async<unit>`
* ExecuteAsync `IDeviantArtAccessToken` `Guid` -> `Task`

### DeviantArtFs.Requests.Collections.Unfave
* AsyncExecute `IDeviantArtAccessToken` `Guid` `IEnumerable<Guid>` -> `Async<int>`
* ExecuteAsync `IDeviantArtAccessToken` `Guid` `IEnumerable<Guid>` -> `Task<int>`

### DeviantArtFs.Requests.Comments.CommentSiblings
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `CommentSiblingsRequest` -> `Async<DeviantArtCommentSiblingsPagedResult>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `CommentSiblingsRequest` -> `Task<IBclDeviantArtCommentSiblingsPagedResult>`

**CommentSiblingsRequest:**

* Commentid: `Guid`
* ExtItem: `bool`

### DeviantArtFs.Requests.Comments.DeviationComments
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `DeviationCommentsRequest` -> `Async<DeviantArtCommentPagedResult>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `DeviationCommentsRequest` -> `Task<IBclDeviantArtCommentPagedResult>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `DeviationCommentsRequest` -> `AsyncSeq<DeviantArtComment>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `DeviationCommentsRequest` -> `Task<IBclDeviantArtComment[]>`

**DeviationCommentsRequest:**

* Deviationid: `Guid`
* Commentid: `Guid?`
* Maxdepth: `int`

### DeviantArtFs.Requests.Comments.PostDeviationComment
* AsyncExecute `IDeviantArtAccessToken` `PostDeviationCommentRequest` -> `Async<DeviantArtComment>`
* ExecuteAsync `IDeviantArtAccessToken` `PostDeviationCommentRequest` -> `Task<IBclDeviantArtComment>`

**PostDeviationCommentRequest:**

* Deviationid: `Guid`
* Body: `string`
* Commentid: `Guid?`

### DeviantArtFs.Requests.Comments.PostProfileComment
* AsyncExecute `IDeviantArtAccessToken` `PostProfileCommentRequest` -> `Async<DeviantArtComment>`
* ExecuteAsync `IDeviantArtAccessToken` `PostProfileCommentRequest` -> `Task<IBclDeviantArtComment>`

**PostProfileCommentRequest:**

* Username: `string`
* Body: `string`
* Commentid: `Guid?`

### DeviantArtFs.Requests.Comments.PostStatusComment
* AsyncExecute `IDeviantArtAccessToken` `PostStatusCommentRequest` -> `Async<DeviantArtComment>`
* ExecuteAsync `IDeviantArtAccessToken` `PostStatusCommentRequest` -> `Task<IBclDeviantArtComment>`

**PostStatusCommentRequest:**

* Statusid: `Guid`
* Body: `string`
* Commentid: `Guid?`

### DeviantArtFs.Requests.Comments.ProfileComments
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `ProfileCommentsRequest` -> `Async<DeviantArtCommentPagedResult>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `ProfileCommentsRequest` -> `Task<IBclDeviantArtCommentPagedResult>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `ProfileCommentsRequest` -> `AsyncSeq<DeviantArtComment>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `ProfileCommentsRequest` -> `Task<IBclDeviantArtComment[]>`

**ProfileCommentsRequest:**

* Username: `string`
* Commentid: `Guid?`
* Maxdepth: `int`

### DeviantArtFs.Requests.Comments.StatusComments
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `StatusCommentsRequest` -> `Async<DeviantArtCommentPagedResult>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `StatusCommentsRequest` -> `Task<IBclDeviantArtCommentPagedResult>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `StatusCommentsRequest` -> `AsyncSeq<DeviantArtComment>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `StatusCommentsRequest` -> `Task<IBclDeviantArtComment[]>`

**StatusCommentsRequest:**

* Statusid: `Guid`
* Commentid: `Guid?`
* Maxdepth: `int`

### DeviantArtFs.Requests.Data.Countries
* AsyncExecute `IDeviantArtAccessToken` -> `Async<IDictionary<int, string>>`
* ExecuteAsync `IDeviantArtAccessToken` -> `Task<IDictionary<int, string>>`

### DeviantArtFs.Requests.Data.Privacy
* AsyncExecute `IDeviantArtAccessToken` -> `Async<string>`
* ExecuteAsync `IDeviantArtAccessToken` -> `Task<string>`

### DeviantArtFs.Requests.Data.Submission
* AsyncExecute `IDeviantArtAccessToken` -> `Async<string>`
* ExecuteAsync `IDeviantArtAccessToken` -> `Task<string>`

### DeviantArtFs.Requests.Data.Tos
* AsyncExecute `IDeviantArtAccessToken` -> `Async<string>`
* ExecuteAsync `IDeviantArtAccessToken` -> `Task<string>`

### DeviantArtFs.Requests.Deviation.Content
* AsyncExecute `IDeviantArtAccessToken` `Guid` -> `Async<DeviationTextContent>`
* ExecuteAsync `IDeviantArtAccessToken` `Guid` -> `Task<IBclDeviationTextContent>`

### DeviantArtFs.Requests.Deviation.DeviationById
* AsyncExecute `IDeviantArtAccessToken` `Guid` -> `Async<Deviation>`
* ExecuteAsync `IDeviantArtAccessToken` `Guid` -> `Task<IBclDeviation>`

### DeviantArtFs.Requests.Deviation.Download
* AsyncExecute `IDeviantArtAccessToken` `Guid` -> `Async<DeviationDownload>`
* ExecuteAsync `IDeviantArtAccessToken` `Guid` -> `Task<IBclDeviationDownload>`

### DeviantArtFs.Requests.Deviation.EmbeddedContent
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `EmbeddedContentRequest` -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `EmbeddedContentRequest` -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `EmbeddedContentRequest` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `EmbeddedContentRequest` -> `Task<IBclDeviation[]>`

**EmbeddedContentRequest:**

* Deviationid: `Guid`
* OffsetDeviationid: `Guid?`

### DeviantArtFs.Requests.Deviation.MetadataById
* AsyncExecute `IDeviantArtAccessToken` `MetadataRequest` -> `Async<IEnumerable<DeviationMetadata>>`
* ExecuteAsync `IDeviantArtAccessToken` `MetadataRequest` -> `Task<IEnumerable<IBclDeviationMetadata>>`

**MetadataRequest:**

* Deviationids: `IEnumerable<Guid>`
* ExtParams: `IDeviantArtExtParams`
* ExtCollection: `bool`

### DeviantArtFs.Requests.Deviation.WhoFaved
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `Guid` -> `Async<DeviantArtPagedResult<DeviantArtWhoFavedUser>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `Guid` -> `Task<IBclDeviantArtPagedResult<IBclDeviantArtWhoFavedUser>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `Guid` -> `AsyncSeq<DeviantArtWhoFavedUser>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `Guid` -> `Task<IBclDeviantArtWhoFavedUser[]>`

### DeviantArtFs.Requests.Feed.FeedHome
* AsyncExecute `IDeviantArtAccessToken` `string option` -> `Async<DeviantArtFeedCursorResult>`
* ExecuteAsync `IDeviantArtAccessToken` `string` -> `Task<IBclDeviantArtFeedCursorResult>`
* ToAsyncSeq `IDeviantArtAccessToken` `string option` -> `AsyncSeq<DeviantArtFeedItem>`
* ToArrayAsync `IDeviantArtAccessToken` `string` `int` -> `Task<IBclDeviantArtFeedItem[]>`

### DeviantArtFs.Requests.Feed.FeedHomeBucket
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `Guid` -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `Guid` -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `Guid` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `Guid` -> `Task<IBclDeviation[]>`

### DeviantArtFs.Requests.Feed.FeedNotifications
* AsyncExecute `IDeviantArtAccessToken` `string option` -> `Async<DeviantArtFeedCursorResult>`
* ExecuteAsync `IDeviantArtAccessToken` `string` -> `Task<IBclDeviantArtFeedCursorResult>`
* ToAsyncSeq `IDeviantArtAccessToken` `string option` -> `AsyncSeq<DeviantArtFeedItem>`
* ToArrayAsync `IDeviantArtAccessToken` `string` `int` -> `Task<IBclDeviantArtFeedItem[]>`

### DeviantArtFs.Requests.Feed.FeedSettings
* AsyncExecute `IDeviantArtAccessToken` -> `Async<DeviantArtFeedSettings>`
* ExecuteAsync `IDeviantArtAccessToken` -> `Task<IBclDeviantArtFeedSettings>`

### DeviantArtFs.Requests.Feed.FeedSettingsUpdate
* AsyncExecute `IDeviantArtAccessToken` `FeedSettingsUpdateRequest` -> `Async<unit>`
* ExecuteAsync `IDeviantArtAccessToken` `FeedSettingsUpdateRequest` -> `Task`

**FeedSettingsUpdateRequest:**

* Statuses: `bool?`
* Deviations: `bool?`
* Journals: `bool?`
* GroupDeviations: `bool?`
* Collections: `bool?`
* Misc: `bool?`

### DeviantArtFs.Requests.Feed.ProfileFeed
* AsyncExecute `IDeviantArtAccessToken` `string option` -> `Async<DeviantArtFeedCursorResult>`
* ExecuteAsync `IDeviantArtAccessToken` `string` -> `Task<IBclDeviantArtFeedCursorResult>`
* ToAsyncSeq `IDeviantArtAccessToken` `string option` -> `AsyncSeq<DeviantArtFeedItem>`
* ToArrayAsync `IDeviantArtAccessToken` `string` `int` -> `Task<IBclDeviantArtFeedItem[]>`

### DeviantArtFs.Requests.Gallery.CreateGalleryFolder
* AsyncExecute `IDeviantArtAccessToken` `string` -> `Async<DeviantArtGalleryFolder>`
* ExecuteAsync `IDeviantArtAccessToken` `string` -> `Task<IBclDeviantArtGalleryFolder>`

### DeviantArtFs.Requests.Gallery.GalleryAllView
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `GalleryAllViewRequest` -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `GalleryAllViewRequest` -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `GalleryAllViewRequest` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `GalleryAllViewRequest` -> `Task<IBclDeviation[]>`

**GalleryAllViewRequest:**

* Username: `string`

### DeviantArtFs.Requests.Gallery.GalleryById
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `GalleryByIdRequest` -> `Async<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `GalleryByIdRequest` -> `Task<IBclDeviantArtPagedResult<IBclDeviation>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `GalleryByIdRequest` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `GalleryByIdRequest` -> `Task<IBclDeviation[]>`

**GalleryByIdRequest:**

* Folderid: `Guid`
* Username: `string`
* Mode: `GalleryRequestMode` (Popular, Newest)

### DeviantArtFs.Requests.Gallery.GalleryFolders
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `GalleryFoldersRequest` -> `Async<DeviantArtPagedResult<DeviantArtGalleryFolder>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `GalleryFoldersRequest` -> `Task<IBclDeviantArtPagedResult<IBclDeviantArtGalleryFolder>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `GalleryFoldersRequest` -> `AsyncSeq<DeviantArtGalleryFolder>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `GalleryFoldersRequest` -> `Task<IBclDeviantArtGalleryFolder[]>`

**GalleryFoldersRequest:**

* Username: `string`
* CalculateSize: `bool`
* ExtPreload: `bool`

### DeviantArtFs.Requests.Gallery.RemoveGalleryFolder
* AsyncExecute `IDeviantArtAccessToken` `Guid` -> `Async<unit>`
* ExecuteAsync `IDeviantArtAccessToken` `Guid` -> `Task`

### DeviantArtFs.Requests.Messages.DeleteMessage
* AsyncExecute `IDeviantArtAccessToken` `DeleteMessageRequest` -> `Async<unit>`
* ExecuteAsync `IDeviantArtAccessToken` `DeleteMessageRequest` -> `Task`

**DeleteMessageRequest:**

* Folderid: `Guid?`
* Messageid: `string`
* Stackid: `string`

### DeviantArtFs.Requests.Messages.FeedbackMessages
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `FeedbackMessagesRequest` -> `Async<DeviantArtPagedResult<DeviantArtMessage>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `FeedbackMessagesRequest` -> `Task<IBclDeviantArtPagedResult<IBclDeviantArtMessage>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `FeedbackMessagesRequest` -> `AsyncSeq<DeviantArtMessage>`
* ToArrayAsync `IDeviantArtAccessToken` `FeedbackMessagesRequest` `int` `int` -> `Task<IBclDeviantArtMessage[]>`

**FeedbackMessagesRequest:**

* Type: `FeedbackMessageType` (Comments, Replies, Activity)
* Folderid: `Guid?`
* Stack: `bool`

### DeviantArtFs.Requests.Messages.FeedbackStack
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `string` -> `Async<DeviantArtPagedResult<DeviantArtMessage>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `string` -> `Task<IBclDeviantArtPagedResult<IBclDeviantArtMessage>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `string` -> `AsyncSeq<DeviantArtMessage>`
* ToArrayAsync `IDeviantArtAccessToken` `string` `int` `int` -> `Task<IBclDeviantArtMessage[]>`

### DeviantArtFs.Requests.Messages.MentionsMessages
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `MentionsMessagesRequest` -> `Async<DeviantArtPagedResult<DeviantArtMessage>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `MentionsMessagesRequest` -> `Task<IBclDeviantArtPagedResult<IBclDeviantArtMessage>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `MentionsMessagesRequest` -> `AsyncSeq<DeviantArtMessage>`
* ToArrayAsync `IDeviantArtAccessToken` `MentionsMessagesRequest` `int` `int` -> `Task<IBclDeviantArtMessage[]>`

**MentionsMessagesRequest:**

* Folderid: `Guid?`
* Stack: `bool`

### DeviantArtFs.Requests.Messages.MentionsStack
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `string` -> `Async<DeviantArtPagedResult<DeviantArtMessage>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `string` -> `Task<IBclDeviantArtPagedResult<IBclDeviantArtMessage>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `string` -> `AsyncSeq<DeviantArtMessage>`
* ToArrayAsync `IDeviantArtAccessToken` `string` `int` `int` -> `Task<IBclDeviantArtMessage[]>`

### DeviantArtFs.Requests.Messages.MessagesFeed
* AsyncExecute `IDeviantArtAccessToken` `string option` `MessagesFeedRequest` -> `Async<DeviantArtMessageCursorResult>`
* ExecuteAsync `IDeviantArtAccessToken` `string` `MessagesFeedRequest` -> `Task<IBclDeviantArtMessageCursorResult>`
* ToAsyncSeq `IDeviantArtAccessToken` `string option` `MessagesFeedRequest` -> `AsyncSeq<DeviantArtMessage>`
* ToArrayAsync `IDeviantArtAccessToken` `MessagesFeedRequest` `string` `int` -> `Task<IBclDeviantArtMessage[]>`

**MessagesFeedRequest:**

* Folderid: `Guid?`
* Stack: `bool`

### DeviantArtFs.Requests.Notes.CreateNotesFolder
* AsyncExecute `IDeviantArtAccessToken` `CreateNotesFolderRequest` -> `Async<DeviantArtNewNotesFolder>`
* ExecuteAsync `IDeviantArtAccessToken` `CreateNotesFolderRequest` -> `Task<IBclDeviantArtNewNotesFolder>`

**CreateNotesFolderRequest:**

* Title: `string`
* Parentid: `Guid?`

### DeviantArtFs.Requests.Notes.DeleteNotes
* AsyncExecute `IDeviantArtAccessToken` `IEnumerable<Guid>` -> `Async<unit>`
* ExecuteAsync `IDeviantArtAccessToken` `IEnumerable<Guid>` -> `Task`

### DeviantArtFs.Requests.Notes.GetNote
* AsyncExecute `IDeviantArtAccessToken` `Guid` -> `Async<DeviantArtNote>`
* ExecuteAsync `IDeviantArtAccessToken` `Guid` -> `Task<IBclDeviantArtNote>`

### DeviantArtFs.Requests.Notes.GetNotes
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `Guid option` -> `Async<DeviantArtPagedResult<DeviantArtNote>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `Guid?` -> `Task<IBclDeviantArtPagedResult<IBclDeviantArtNote>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `Guid option` -> `AsyncSeq<DeviantArtNote>`
* ToArrayAsync `IDeviantArtAccessToken` `Guid?` `int` `int` -> `Task<IBclDeviantArtNote[]>`

### DeviantArtFs.Requests.Notes.MarkNotes
* AsyncExecute `IDeviantArtAccessToken` `MarkNotesRequest` -> `Async<unit>`
* ExecuteAsync `IDeviantArtAccessToken` `MarkNotesRequest` -> `Task`

**MarkNotesRequest:**

* Noteids: `IEnumerable<Guid>`
* MarkAs: `MarkAs` (Read, Unread, Starred, NotStarred, Spam)

### DeviantArtFs.Requests.Notes.MoveNotes
* AsyncExecute `IDeviantArtAccessToken` `MoveNotesRequest` -> `Async<unit>`
* ExecuteAsync `IDeviantArtAccessToken` `MoveNotesRequest` -> `Task`

**MoveNotesRequest:**

* Noteids: `IEnumerable<Guid>`
* Folderid: `Guid`

### DeviantArtFs.Requests.Notes.NotesFolders
* AsyncExecute `IDeviantArtAccessToken` -> `Async<FSharpList<DeviantArtNotesFolder>>`
* ExecuteAsync `IDeviantArtAccessToken` -> `Task<IEnumerable<IBclDeviantArtNotesFolder>>`

### DeviantArtFs.Requests.Notes.RemoveNotesFolder
* AsyncExecute `IDeviantArtAccessToken` `Guid` -> `Async<unit>`
* ExecuteAsync `IDeviantArtAccessToken` `Guid` -> `Task`

### DeviantArtFs.Requests.Notes.RenameNotesFolder
* AsyncExecute `IDeviantArtAccessToken` `RenameNotesFolderRequest` -> `Async<DeviantArtRenamedNotesFolder>`
* ExecuteAsync `IDeviantArtAccessToken` `RenameNotesFolderRequest` -> `Task<IBclDeviantArtRenamedNotesFolder>`

**RenameNotesFolderRequest:**

* Folderid: `Guid`
* Title: `string`

### DeviantArtFs.Requests.Notes.SendNote
* AsyncExecute `IDeviantArtAccessToken` `SendNoteRequest` -> `Async<FSharpList<DeviantArtSendNoteResult>>`
* ExecuteAsync `IDeviantArtAccessToken` `SendNoteRequest` -> `Task<IEnumerable<IBclDeviantArtSendNoteResult>>`

**SendNoteRequest:**

* To: `IEnumerable<string>`
* Subject: `string`
* Body: `string`
* Noteid: `Guid?`

### DeviantArtFs.Requests.Stash.Contents
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `long` -> `Async<DeviantArtPagedResult<StashMetadata>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `long` -> `Task<IBclDeviantArtPagedResult<IBclStashMetadata>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `long` -> `AsyncSeq<StashMetadata>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `long` -> `Task<IBclStashMetadata[]>`

### DeviantArtFs.Requests.Stash.Delete
* AsyncExecute `IDeviantArtAccessToken` `long` -> `Async<unit>`
* ExecuteAsync `IDeviantArtAccessToken` `long` -> `Task`

### DeviantArtFs.Requests.Stash.Delta
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `DeltaRequest` -> `Async<StashDeltaResult>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `DeltaRequest` -> `Task<IBclStashDeltaResult>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `DeltaRequest` -> `AsyncSeq<StashDeltaEntry>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `DeltaRequest` -> `Task<IBclStashDeltaEntry[]>`

**DeltaRequest:**

* Cursor: `string`
* ExtParams: `IDeviantArtExtParams`

### DeviantArtFs.Requests.Stash.Item
* AsyncExecute `IDeviantArtAccessToken` `ItemRequest` -> `Async<StashMetadata>`
* ExecuteAsync `IDeviantArtAccessToken` `ItemRequest` -> `Task<IBclStashMetadata>`

**ItemRequest:**

* Itemid: `long`
* ExtParams: `IDeviantArtExtParams`

### DeviantArtFs.Requests.Stash.Move
* AsyncExecute `IDeviantArtAccessToken` `long` `long` -> `Async<StashMoveResult>`
* ExecuteAsync `IDeviantArtAccessToken` `long` `long` -> `Task<IBclStashMoveResult>`

### DeviantArtFs.Requests.Stash.Position
* AsyncExecute `IDeviantArtAccessToken` `long` `int` -> `Async<unit>`
* ExecuteAsync `IDeviantArtAccessToken` `long` `int` -> `Task`

### DeviantArtFs.Requests.Stash.Publish
* AsyncExecute `IDeviantArtAccessToken` `PublishRequest` -> `Async<StashPublishResponse>`
* ExecuteAsync `IDeviantArtAccessToken` `PublishRequest` -> `Task<IBclStashPublishResponse>`

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
* AsyncExecute `IDeviantArtAccessToken` `PublishCategoryTreeRequest` -> `Async<IEnumerable<DeviantArtCategory>>`
* ExecuteAsync `IDeviantArtAccessToken` `PublishCategoryTreeRequest` -> `Task<IEnumerable<IBclDeviantArtCategory>>`

**PublishCategoryTreeRequest:**

* Catpath: `string`
* Filetype: `string`
* Frequent: `bool`

### DeviantArtFs.Requests.Stash.PublishUserdata
* AsyncExecute `IDeviantArtAccessToken` -> `Async<StashPublishUserdataResult>`
* ExecuteAsync `IDeviantArtAccessToken` -> `Task<IBclStashPublishUserdataResult>`

### DeviantArtFs.Requests.Stash.Space
* AsyncExecute `IDeviantArtAccessToken` -> `Async<StashSpaceResult>`
* ExecuteAsync `IDeviantArtAccessToken` -> `Task<IBclStashSpaceResult>`

### DeviantArtFs.Requests.Stash.Stack
* AsyncExecute `IDeviantArtAccessToken` `long` -> `Async<StashMetadata>`
* ExecuteAsync `IDeviantArtAccessToken` `long` -> `Task<IBclStashMetadata>`

### DeviantArtFs.Requests.Stash.Submit
* AsyncExecute `IDeviantArtAccessToken` `SubmitRequest` -> `Async<long>`
* ExecuteAsync `IDeviantArtAccessToken` `SubmitRequest` -> `Task<long>`

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
* AsyncExecute `IDeviantArtAccessToken` `UpdateRequest` -> `Async<unit>`
* ExecuteAsync `IDeviantArtAccessToken` `UpdateRequest` -> `Task`

**UpdateRequest:**

* Stackid: `long`
* Title: `DeviantArtFieldChange<string>`
* Description: `DeviantArtFieldChange<string>`

### DeviantArtFs.Requests.User.dAmnToken
* AsyncExecute `IDeviantArtAccessToken` -> `Async<string>`
* ExecuteAsync `IDeviantArtAccessToken` -> `Task<string>`

### DeviantArtFs.Requests.User.Friends
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `FriendsRequest` -> `Async<DeviantArtPagedResult<DeviantArtFriendRecord>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `FriendsRequest` -> `Task<IBclDeviantArtPagedResult<IBclDeviantArtFriendRecord>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `FriendsRequest` -> `AsyncSeq<DeviantArtFriendRecord>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `FriendsRequest` -> `Task<IBclDeviantArtFriendRecord[]>`

**FriendsRequest:**

* Username: `string`

### DeviantArtFs.Requests.User.FriendsSearch
* AsyncExecute `IDeviantArtAccessToken` `FriendsSearchRequest` -> `Async<FSharpList<DeviantArtUser>>`
* ExecuteAsync `IDeviantArtAccessToken` `FriendsSearchRequest` -> `Task<IEnumerable<IBclDeviantArtUser>>`

**FriendsSearchRequest:**

* Query: `string`
* Username: `string`

### DeviantArtFs.Requests.User.FriendsUnwatch
* AsyncExecute `IDeviantArtAccessToken` `string` -> `Async<unit>`
* ExecuteAsync `IDeviantArtAccessToken` `string` -> `Task`

### DeviantArtFs.Requests.User.FriendsWatch
* AsyncExecute `IDeviantArtAccessToken` `FriendsWatchRequest` -> `Async<unit>`
* ExecuteAsync `IDeviantArtAccessToken` `FriendsWatchRequest` -> `Task`

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
* AsyncExecute `IDeviantArtAccessToken` `string` -> `Async<bool>`
* ExecuteAsync `IDeviantArtAccessToken` `string` -> `Task<bool>`

### DeviantArtFs.Requests.User.ProfileByName
* AsyncExecute `IDeviantArtAccessToken` `ProfileByNameRequest` -> `Async<DeviantArtProfile>`
* ExecuteAsync `IDeviantArtAccessToken` `ProfileByNameRequest` -> `Task<IBclDeviantArtProfile>`

**ProfileByNameRequest:**

* Username: `string`
* ExtCollections: `bool`
* ExtGalleries: `bool`

### DeviantArtFs.Requests.User.ProfileUpdate
* AsyncExecute `IDeviantArtAccessToken` `ProfileUpdateRequest` -> `Async<unit>`
* ExecuteAsync `IDeviantArtAccessToken` `ProfileUpdateRequest` -> `Task`

**ProfileUpdateRequest:**

* UserIsArtist: `DeviantArtFieldChange<bool>`
* ArtistLevel: `DeviantArtFieldChange<ArtistLevel>`
* ArtistSpecialty: `DeviantArtFieldChange<ArtistSpecialty>`
* RealName: `DeviantArtFieldChange<string>`
* Tagline: `DeviantArtFieldChange<string>`
* Countryid: `DeviantArtFieldChange<int>`
* Website: `DeviantArtFieldChange<string>`
* Bio: `DeviantArtFieldChange<string>`

### DeviantArtFs.Requests.User.StatusById
* AsyncExecute `IDeviantArtAccessToken` `Guid` -> `Async<DeviantArtStatus>`
* ExecuteAsync `IDeviantArtAccessToken` `Guid` -> `Task<IBclDeviantArtStatus>`

### DeviantArtFs.Requests.User.StatusesList
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `string` -> `Async<DeviantArtPagedResult<DeviantArtStatus>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `string` -> `Task<IBclDeviantArtPagedResult<IBclDeviantArtStatus>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `string` -> `AsyncSeq<DeviantArtStatus>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `string` -> `Task<IBclDeviantArtStatus[]>`

### DeviantArtFs.Requests.User.StatusPost
* AsyncExecute `IDeviantArtAccessToken` `StatusPostRequest` -> `Async<Guid>`
* ExecuteAsync `IDeviantArtAccessToken` `StatusPostRequest` -> `Task<Guid>`

**StatusPostRequest:**

* Body: `string`
* Statusid: `Guid?`
* Parentid: `Guid?`
* Stashid: `long?`

### DeviantArtFs.Requests.User.Watchers
* AsyncExecute `IDeviantArtAccessToken` `IDeviantArtPagingParams` `WatchersRequest` -> `Async<DeviantArtPagedResult<DeviantArtWatcherRecord>>`
* ExecuteAsync `IDeviantArtAccessToken` `IDeviantArtPagingParams` `WatchersRequest` -> `Task<IBclDeviantArtPagedResult<IBclDeviantArtWatcherRecord>>`
* ToAsyncSeq `IDeviantArtAccessToken` `int` `WatchersRequest` -> `AsyncSeq<DeviantArtWatcherRecord>`
* ToArrayAsync `IDeviantArtAccessToken` `int` `int` `WatchersRequest` -> `Task<IBclDeviantArtWatcherRecord[]>`

**WatchersRequest:**

* Username: `string`

### DeviantArtFs.Requests.User.Whoami
* AsyncExecute `IDeviantArtAccessToken` -> `Async<DeviantArtUser>`
* ExecuteAsync `IDeviantArtAccessToken` -> `Task<IBclDeviantArtUser>`

### DeviantArtFs.Requests.User.Whois
* AsyncExecute `IDeviantArtAccessToken` `IEnumerable<string>` -> `Async<FSharpList<DeviantArtUser>>`
* ExecuteAsync `IDeviantArtAccessToken` `IEnumerable<string>` -> `Task<IEnumerable<IBclDeviantArtUser>>`

### DeviantArtFs.Requests.Util.Placebo
* AsyncIsValid `IDeviantArtAccessToken` -> `Async<bool>`
* IsValidAsync `IDeviantArtAccessToken` -> `Task<bool>`

