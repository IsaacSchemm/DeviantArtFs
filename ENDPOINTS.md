This is a list of functions in the DeviantArtFs library that call DeviantArt / Sta.sh API endpoints.

Methods that return an Async<T> or AsyncSeq<T> are intended for F# consumers. Methods that return a Task<T> can be used from async methods in C# and VB.NET.

A question mark (?) following a type name indicates a Nullable<T>, as in C#.

### DeviantArtFs.Api.Browse.DailyDeviations
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `DailyDeviationsRequest req` -> `AsyncSeq<FSharpList<Deviation>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `DailyDeviationsRequest req` -> `Task<FSharpList<Deviation>>`

**DailyDeviationsRequest:**

* Date: `DateTime?`

### DeviantArtFs.Api.Browse.DeviantsYouWatch
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `DeviantArtPagingParams paging` -> `Task<DeviantArtPagedResult<Deviation>>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Int32 offset` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Int32 offset` `Int32 limit` -> `Task<Deviation[]>`

### DeviantArtFs.Api.Browse.MoreLikeThisPreview
* AsyncExecute `IDeviantArtAccessToken token` `Guid seed` -> `AsyncSeq<DeviantArtMoreLikeThisPreviewResult>`
* ExecuteAsync `IDeviantArtAccessToken token` `Guid seed` -> `Task<DeviantArtMoreLikeThisPreviewResult>`

### DeviantArtFs.Api.Browse.Newest
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `NewestRequest req` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtBrowsePagedResult>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `NewestRequest req` `DeviantArtPagingParams paging` -> `Task<DeviantArtBrowsePagedResult>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `NewestRequest req` `Int32 offset` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `NewestRequest req` `Int32 offset` `Int32 limit` -> `Task<Deviation[]>`

**NewestRequest:**

* CategoryPath: `String`
* Q: `String`

### DeviantArtFs.Api.Browse.Popular
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `PopularRequest req` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtBrowsePagedResult>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `PopularRequest req` `DeviantArtPagingParams paging` -> `Task<DeviantArtBrowsePagedResult>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `PopularRequest req` `Int32 offset` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `PopularRequest req` `Int32 offset` `Int32 limit` -> `Task<Deviation[]>`

**PopularRequest:**

* CategoryPath: `String`
* Q: `String`
* Timerange: `PopularTimeRange` (EightHours, TwentyFourHours, ThreeDays, OneWeek, OneMonth, AllTime)

### DeviantArtFs.Api.Browse.PostsByDeviantsYouWatch
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtPagedResult<DeviantArtPost>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `DeviantArtPagingParams paging` -> `Task<DeviantArtPagedResult<DeviantArtPost>>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Int32 offset` -> `AsyncSeq<DeviantArtPost>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Int32 offset` `Int32 limit` -> `Task<DeviantArtPost[]>`

### DeviantArtFs.Api.Browse.Recommended
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `RecommendedRequest req` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtRecommendedPagedResult>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `RecommendedRequest req` `DeviantArtPagingParams paging` -> `Task<DeviantArtRecommendedPagedResult>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `RecommendedRequest req` `Int32 offset` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `RecommendedRequest req` `Int32 offset` `Int32 limit` -> `Task<Deviation[]>`

**RecommendedRequest:**

* Q: `String`

### DeviantArtFs.Api.Browse.Tags
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `String tag` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtBrowsePagedResult>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `String tag` `DeviantArtPagingParams paging` -> `Task<DeviantArtBrowsePagedResult>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `String tag` `Int32 offset` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `String tag` `Int32 offset` `Int32 limit` -> `Task<Deviation[]>`

### DeviantArtFs.Api.Browse.TagsSearch
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `String tag_name` -> `AsyncSeq<IEnumerable<String>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `String tag_name` -> `Task<IEnumerable<String>>`

### DeviantArtFs.Api.Browse.Topic
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `String topic` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `String topic` `DeviantArtPagingParams paging` -> `Task<DeviantArtPagedResult<Deviation>>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `String topic` `Int32 offset` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `String topic` `Int32 offset` `Int32 limit` -> `Task<Deviation[]>`

### DeviantArtFs.Api.Browse.Topics
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `TopicsRequest req` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtPagedResult<DeviantArtTopic>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `TopicsRequest req` `DeviantArtPagingParams paging` -> `Task<DeviantArtPagedResult<DeviantArtTopic>>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `TopicsRequest req` `Int32 offset` -> `AsyncSeq<DeviantArtTopic>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `TopicsRequest req` `Int32 offset` `Int32 limit` -> `Task<DeviantArtTopic[]>`

**TopicsRequest:**

* NumDeviationsPerTopic: `Int32?`

### DeviantArtFs.Api.Browse.TopTopics
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` -> `AsyncSeq<FSharpList<DeviantArtTopic>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` -> `Task<FSharpList<DeviantArtTopic>>`

### DeviantArtFs.Api.Browse.UserJournals
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `UserJournalsRequest req` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `UserJournalsRequest req` `DeviantArtPagingParams paging` -> `Task<DeviantArtPagedResult<Deviation>>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `UserJournalsRequest req` `Int32 offset` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `UserJournalsRequest req` `Int32 offset` `Int32 limit` -> `Task<Deviation[]>`

**UserJournalsRequest:**

* Username: `String`
* Featured: `Boolean`

### DeviantArtFs.Api.Collections.CollectionById
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `CollectionByIdRequest req` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtFolderPagedResult>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `CollectionByIdRequest req` `DeviantArtPagingParams paging` -> `Task<DeviantArtFolderPagedResult>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `CollectionByIdRequest req` `Int32 offset` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `CollectionByIdRequest req` `Int32 offset` `Int32 limit` -> `Task<Deviation[]>`

**CollectionByIdRequest:**

* Folderid: `Guid`
* Username: `String`

### DeviantArtFs.Api.Collections.CollectionFolders
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `CollectionFoldersRequest req` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtPagedResult<DeviantArtCollectionFolder>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `CollectionFoldersRequest req` `DeviantArtPagingParams paging` -> `Task<DeviantArtPagedResult<DeviantArtCollectionFolder>>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `CollectionFoldersRequest req` `DeviantArtPagingParams offset` -> `AsyncSeq<DeviantArtCollectionFolder>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `CollectionFoldersRequest req` `DeviantArtPagingParams offset` `Int32 limit` -> `Task<DeviantArtCollectionFolder[]>`

**CollectionFoldersRequest:**

* Username: `String`
* CalculateSize: `Boolean`
* ExtPreload: `Boolean`

### DeviantArtFs.Api.Collections.CreateCollectionFolder
* AsyncExecute `IDeviantArtAccessToken token` `String folder` -> `AsyncSeq<DeviantArtCollectionFolder>`
* ExecuteAsync `IDeviantArtAccessToken token` `String folder` -> `Task<DeviantArtCollectionFolder>`

### DeviantArtFs.Api.Collections.Fave
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Guid deviationid` `IEnumerable<Guid> folderids` -> `AsyncSeq<FaveResponse>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Guid deviationid` `IEnumerable<Guid> folderids` -> `Task<FaveResponse>`

**Guid:**


### DeviantArtFs.Api.Collections.RemoveCollectionFolder
* AsyncExecute `IDeviantArtAccessToken token` `Guid folderid` -> `AsyncSeq<DeviantArtSuccessOrErrorResponse>`
* ExecuteAsync `IDeviantArtAccessToken token` `Guid folderid` -> `Task<DeviantArtSuccessOrErrorResponse>`

### DeviantArtFs.Api.Collections.Unfave
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Guid deviationid` `IEnumerable<Guid> folderids` -> `AsyncSeq<UnfaveResponse>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Guid deviationid` `IEnumerable<Guid> folderids` -> `Task<UnfaveResponse>`

**Guid:**


### DeviantArtFs.Api.Comments.CommentSiblings
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `CommentSiblingsRequest req` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtCommentSiblingsPagedResult>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `CommentSiblingsRequest req` `DeviantArtPagingParams paging` -> `Task<DeviantArtCommentSiblingsPagedResult>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `CommentSiblingsRequest req` `Int32 offset` -> `AsyncSeq<DeviantArtComment>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `CommentSiblingsRequest req` `Int32 offset` `Int32 limit` -> `Task<DeviantArtComment[]>`

**CommentSiblingsRequest:**

* Commentid: `Guid`
* ExtItem: `Boolean`

### DeviantArtFs.Api.Comments.DeviationComments
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `DeviationCommentsRequest req` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtCommentPagedResult>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `DeviationCommentsRequest req` `DeviantArtPagingParams paging` -> `Task<DeviantArtCommentPagedResult>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `DeviationCommentsRequest req` `Int32 offset` -> `AsyncSeq<DeviantArtComment>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `DeviationCommentsRequest req` `Int32 offset` `Int32 limit` -> `Task<DeviantArtComment[]>`

**DeviationCommentsRequest:**

* Deviationid: `Guid`
* Commentid: `Guid?`
* Maxdepth: `Int32`

### DeviantArtFs.Api.Comments.PostDeviationComment
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `PostDeviationCommentRequest req` -> `AsyncSeq<DeviantArtComment>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `PostDeviationCommentRequest req` -> `Task<DeviantArtComment>`

**PostDeviationCommentRequest:**

* Deviationid: `Guid`
* Body: `String`
* Commentid: `Guid?`

### DeviantArtFs.Api.Comments.PostProfileComment
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `PostProfileCommentRequest req` -> `AsyncSeq<DeviantArtComment>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `PostProfileCommentRequest req` -> `Task<DeviantArtComment>`

**PostProfileCommentRequest:**

* Username: `String`
* Body: `String`
* Commentid: `Guid?`

### DeviantArtFs.Api.Comments.PostStatusComment
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `PostStatusCommentRequest req` -> `AsyncSeq<DeviantArtComment>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `PostStatusCommentRequest req` -> `Task<DeviantArtComment>`

**PostStatusCommentRequest:**

* Statusid: `Guid`
* Body: `String`
* Commentid: `Guid?`

### DeviantArtFs.Api.Comments.ProfileComments
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `ProfileCommentsRequest req` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtCommentPagedResult>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `ProfileCommentsRequest req` `DeviantArtPagingParams paging` -> `Task<DeviantArtCommentPagedResult>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `ProfileCommentsRequest req` `Int32 offset` -> `AsyncSeq<DeviantArtComment>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `ProfileCommentsRequest req` `Int32 offset` `Int32 limit` -> `Task<DeviantArtComment[]>`

**ProfileCommentsRequest:**

* Username: `String`
* Commentid: `Guid?`
* Maxdepth: `Int32`

### DeviantArtFs.Api.Comments.StatusComments
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `StatusCommentsRequest req` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtCommentPagedResult>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `StatusCommentsRequest req` `DeviantArtPagingParams paging` -> `Task<DeviantArtCommentPagedResult>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `StatusCommentsRequest req` `Int32 offset` -> `AsyncSeq<DeviantArtComment>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `StatusCommentsRequest req` `Int32 offset` `Int32 limit` -> `Task<DeviantArtComment[]>`

**StatusCommentsRequest:**

* Statusid: `Guid`
* Commentid: `Guid?`
* Maxdepth: `Int32`

### DeviantArtFs.Api.Data.Countries
* AsyncExecute `IDeviantArtAccessToken token` -> `AsyncSeq<FSharpList<CountriesElement>>`
* ExecuteAsync `IDeviantArtAccessToken token` -> `Task<FSharpList<CountriesElement>>`

### DeviantArtFs.Api.Data.Privacy
* AsyncExecute `IDeviantArtAccessToken token` -> `AsyncSeq<String>`
* ExecuteAsync `IDeviantArtAccessToken token` -> `Task<String>`

### DeviantArtFs.Api.Data.Submission
* AsyncExecute `IDeviantArtAccessToken token` -> `AsyncSeq<String>`
* ExecuteAsync `IDeviantArtAccessToken token` -> `Task<String>`

### DeviantArtFs.Api.Data.Tos
* AsyncExecute `IDeviantArtAccessToken token` -> `AsyncSeq<String>`
* ExecuteAsync `IDeviantArtAccessToken token` -> `Task<String>`

### DeviantArtFs.Api.Deviation.Content
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Guid deviationid` -> `AsyncSeq<DeviationTextContent>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Guid deviationid` -> `Task<DeviationTextContent>`

### DeviantArtFs.Api.Deviation.DeviationById
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Guid id` -> `AsyncSeq<Deviation>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Guid id` -> `Task<Deviation>`

### DeviantArtFs.Api.Deviation.Download
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Guid deviationid` -> `AsyncSeq<DeviationDownload>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Guid deviationid` -> `Task<DeviationDownload>`

### DeviantArtFs.Api.Deviation.EmbeddedContent
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `EmbeddedContentRequest req` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtEmbeddedContentPagedResult>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `EmbeddedContentRequest req` `DeviantArtPagingParams paging` -> `Task<DeviantArtEmbeddedContentPagedResult>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `EmbeddedContentRequest req` `Int32 offset` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `EmbeddedContentRequest req` `Int32 offset` `Int32 limit` -> `Task<Deviation[]>`

**EmbeddedContentRequest:**

* Deviationid: `Guid`
* OffsetDeviationid: `Guid?`

### DeviantArtFs.Api.Deviation.MetadataById
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `MetadataRequest req` -> `AsyncSeq<FSharpList<DeviationMetadata>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `MetadataRequest req` -> `Task<FSharpList<DeviationMetadata>>`

**MetadataRequest:**

* Deviationids: `IEnumerable<Guid>`
* ExtParams: `DeviantArtExtParams`
* ExtCollection: `Boolean`

### DeviantArtFs.Api.Deviation.WhoFaved
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Guid deviationid` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtPagedResult<DeviantArtWhoFavedUser>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Guid req` `DeviantArtPagingParams paging` -> `Task<DeviantArtPagedResult<DeviantArtWhoFavedUser>>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Guid req` `Int32 offset` -> `AsyncSeq<DeviantArtWhoFavedUser>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Guid offset` `Int32 limit` `Int32 deviationid` -> `Task<DeviantArtWhoFavedUser[]>`

### DeviantArtFs.Api.Gallery.CreateGalleryFolder
* AsyncExecute `IDeviantArtAccessToken token` `String folder` -> `AsyncSeq<DeviantArtCollectionFolder>`
* ExecuteAsync `IDeviantArtAccessToken token` `String folder` -> `Task<DeviantArtCollectionFolder>`

### DeviantArtFs.Api.Gallery.GalleryAllView
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `GalleryAllViewRequest req` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtPagedResult<Deviation>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `GalleryAllViewRequest req` `DeviantArtPagingParams paging` -> `Task<DeviantArtPagedResult<Deviation>>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `GalleryAllViewRequest req` `Int32 offset` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `GalleryAllViewRequest req` `Int32 offset` `Int32 limit` -> `Task<Deviation[]>`

**GalleryAllViewRequest:**

* Username: `String`

### DeviantArtFs.Api.Gallery.GalleryById
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `GalleryByIdRequest req` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtFolderPagedResult>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `GalleryByIdRequest req` `DeviantArtPagingParams paging` -> `Task<DeviantArtFolderPagedResult>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `GalleryByIdRequest req` `Int32 offset` -> `AsyncSeq<Deviation>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `GalleryByIdRequest req` `Int32 offset` `Int32 limit` -> `Task<Deviation[]>`

**GalleryByIdRequest:**

* Folderid: `Guid?`
* Username: `String`
* Mode: `GalleryRequestMode` (Popular | Newest)

### DeviantArtFs.Api.Gallery.GalleryFolders
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `GalleryFoldersRequest req` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtPagedResult<DeviantArtGalleryFolder>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `GalleryFoldersRequest req` `DeviantArtPagingParams paging` -> `Task<DeviantArtPagedResult<DeviantArtGalleryFolder>>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `GalleryFoldersRequest req` `Int32 offset` -> `AsyncSeq<DeviantArtGalleryFolder>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `GalleryFoldersRequest req` `Int32 offset` `Int32 limit` -> `Task<DeviantArtGalleryFolder[]>`

**GalleryFoldersRequest:**

* Username: `String`
* CalculateSize: `Boolean`
* ExtPreload: `Boolean`

### DeviantArtFs.Api.Gallery.RemoveGalleryFolder
* AsyncExecute `IDeviantArtAccessToken token` `Guid folderid` -> `AsyncSeq<DeviantArtSuccessOrErrorResponse>`
* ExecuteAsync `IDeviantArtAccessToken token` `Guid folderid` -> `Task<DeviantArtSuccessOrErrorResponse>`

### DeviantArtFs.Api.Messages.DeleteMessage
* AsyncExecute `IDeviantArtAccessToken token` `DeleteMessageRequest req` -> `AsyncSeq<DeviantArtSuccessOrErrorResponse>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeleteMessageRequest req` -> `Task<DeviantArtSuccessOrErrorResponse>`

**DeleteMessageRequest:**

* Folderid: `Guid?`
* Messageid: `String`
* Stackid: `String`

### DeviantArtFs.Api.Messages.FeedbackMessages
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `FeedbackMessagesRequest req` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtPagedResult<DeviantArtMessage>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `FeedbackMessagesRequest req` `DeviantArtPagingParams paging` -> `Task<DeviantArtPagedResult<DeviantArtMessage>>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `FeedbackMessagesRequest req` `Int32 offset` -> `AsyncSeq<DeviantArtMessage>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `FeedbackMessagesRequest req` `Int32 offset` `Int32 limit` -> `Task<DeviantArtMessage[]>`

**FeedbackMessagesRequest:**

* Type: `FeedbackMessageType` (Comments, Replies, Activity)
* Folderid: `Guid?`
* Stack: `Boolean`

### DeviantArtFs.Api.Messages.FeedbackStack
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `String stackid` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtPagedResult<DeviantArtMessage>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `String stackid` `DeviantArtPagingParams paging` -> `Task<DeviantArtPagedResult<DeviantArtMessage>>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Int32 offset` `String stackid` -> `AsyncSeq<DeviantArtMessage>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Int32 stackid` `String offset` `Int32 limit` -> `Task<DeviantArtMessage[]>`

### DeviantArtFs.Api.Messages.MentionsMessages
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `MentionsMessagesRequest req` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtPagedResult<DeviantArtMessage>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `MentionsMessagesRequest req` `DeviantArtPagingParams paging` -> `Task<DeviantArtPagedResult<DeviantArtMessage>>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `MentionsMessagesRequest req` `Int32 offset` -> `AsyncSeq<DeviantArtMessage>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `MentionsMessagesRequest req` `Int32 offset` `Int32 limit` -> `Task<DeviantArtMessage[]>`

**MentionsMessagesRequest:**

* Folderid: `Guid?`
* Stack: `Boolean`

### DeviantArtFs.Api.Messages.MentionsStack
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `String stackid` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtPagedResult<DeviantArtMessage>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `String stackid` `DeviantArtPagingParams paging` -> `Task<DeviantArtPagedResult<DeviantArtMessage>>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `String stackid` `Int32 offset` -> `AsyncSeq<DeviantArtMessage>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `String stackid` `Int32 offset` `Int32 limit` -> `Task<DeviantArtMessage[]>`

### DeviantArtFs.Api.Messages.MessagesFeed
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `MessagesFeedRequest req` `FSharpOption<String> cursor` -> `AsyncSeq<DeviantArtMessageCursorResult>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `MessagesFeedRequest req` `String cursor` -> `Task<DeviantArtMessageCursorResult>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `MessagesFeedRequest req` `FSharpOption<String> cursor` -> `AsyncSeq<DeviantArtMessage>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `MessagesFeedRequest req` `String cursor` `Int32 limit` -> `Task<DeviantArtMessage[]>`

**MessagesFeedRequest:**

* Folderid: `Guid?`
* Stack: `Boolean`

**String:**

* Chars: `Char`
* Length: `Int32`

### DeviantArtFs.Api.Stash.Contents
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Int64 stackid` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtPagedResult<StashMetadata>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Int64 stackid` `DeviantArtPagingParams paging` -> `Task<DeviantArtPagedResult<StashMetadata>>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Int64 stackid` `Int32 offset` -> `AsyncSeq<StashMetadata>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Int64 stackid` `Int32 offset` `Int32 limit` -> `Task<StashMetadata[]>`

### DeviantArtFs.Api.Stash.Delete
* AsyncExecute `IDeviantArtAccessToken token` `Int64 itemid` -> `AsyncSeq<DeviantArtSuccessOrErrorResponse>`
* ExecuteAsync `IDeviantArtAccessToken token` `Int64 itemid` -> `Task<DeviantArtSuccessOrErrorResponse>`

### DeviantArtFs.Api.Stash.Delta
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `DeltaRequest req` `DeviantArtPagingParams paging` -> `AsyncSeq<StashDeltaResult>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `DeltaRequest req` `DeviantArtPagingParams paging` -> `Task<StashDeltaResult>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `DeltaRequest req` `Int32 offset` -> `AsyncSeq<StashDeltaEntry>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `DeltaRequest req` `Int32 offset` `Int32 limit` -> `Task<StashDeltaEntry[]>`

**DeltaRequest:**

* Cursor: `String`
* ExtParams: `DeviantArtExtParams`

### DeviantArtFs.Api.Stash.Item
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `ItemRequest req` -> `AsyncSeq<StashMetadata>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `ItemRequest req` -> `Task<StashMetadata>`

**ItemRequest:**

* Itemid: `Int64`
* ExtParams: `DeviantArtExtParams`

### DeviantArtFs.Api.Stash.Move
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Int64 stackid` `Int64 targetid` -> `AsyncSeq<StashMoveResult>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Int64 stackid` `Int64 targetid` -> `Task<StashMoveResult>`

### DeviantArtFs.Api.Stash.Position
* AsyncExecute `IDeviantArtAccessToken token` `Int64 stackid` `Int32 position` -> `AsyncSeq<DeviantArtSuccessOrErrorResponse>`
* ExecuteAsync `IDeviantArtAccessToken token` `Int64 stackid` `Int32 position` -> `Task<DeviantArtSuccessOrErrorResponse>`

### DeviantArtFs.Api.Stash.Publish
* AsyncExecute `IDeviantArtAccessToken token` `PublishRequest req` -> `AsyncSeq<StashPublishResponse>`
* ExecuteAsync `IDeviantArtAccessToken token` `PublishRequest req` -> `Task<StashPublishResponse>`

**PublishRequest:**

* IsMature: `Boolean`
* MatureLevel: `MatureLevel` (None | Strict | Moderate)
* MatureClassification: `IEnumerable<MatureClassification>`
* AgreeSubmission: `Boolean`
* AgreeTos: `Boolean`
* Catpath: `String`
* Feature: `Boolean`
* AllowComments: `Boolean`
* RequestCritique: `Boolean`
* DisplayResolution: `DisplayResolution` (Original, Max400Px, Max600px, Max800px, Max900px, Max1024px, Max1280px, Max1600px, Max1920px)
* Sharing: `Sharing` (Allow | HideShareButtons | HideAndMembersOnly)
* LicenseOptions: `LicenseOptions`
* Galleryids: `IEnumerable<Guid>`
* AllowFreeDownload: `Boolean`
* AddWatermark: `Boolean`
* Itemid: `Int64`

### DeviantArtFs.Api.Stash.PublishCategoryTree
* AsyncExecute `IDeviantArtAccessToken token` `PublishCategoryTreeRequest req` -> `AsyncSeq<DeviantArtCategoryList>`
* ExecuteAsync `IDeviantArtAccessToken token` `PublishCategoryTreeRequest req` -> `Task<DeviantArtCategoryList>`

**PublishCategoryTreeRequest:**

* Catpath: `String`
* Filetype: `String`
* Frequent: `Boolean`

### DeviantArtFs.Api.Stash.PublishUserdata
* AsyncExecute `IDeviantArtAccessToken token` -> `AsyncSeq<StashPublishUserdataResult>`
* ExecuteAsync `IDeviantArtAccessToken token` -> `Task<StashPublishUserdataResult>`

### DeviantArtFs.Api.Stash.Space
* AsyncExecute `IDeviantArtAccessToken token` -> `AsyncSeq<StashSpaceResult>`
* ExecuteAsync `IDeviantArtAccessToken token` -> `Task<StashSpaceResult>`

### DeviantArtFs.Api.Stash.Stack
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Int64 stackid` -> `AsyncSeq<StashMetadata>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Int64 stackid` -> `Task<StashMetadata>`

### DeviantArtFs.Api.Stash.Submit
* AsyncExecute `IDeviantArtAccessToken token` `SubmitRequest ps` -> `AsyncSeq<SubmitResponse>`
* ExecuteAsync `IDeviantArtAccessToken token` `SubmitRequest ps` -> `Task<SubmitResponse>`

**SubmitRequest:**

* Filename: `String`
* ContentType: `String`
* Data: `Byte[]`
* Title: `String`
* ArtistComments: `String`
* Tags: `IEnumerable<String>`
* OriginalUrl: `String`
* IsDirty: `Boolean?`
* Itemid: `Int64?`
* Stack: `String`
* Stackid: `Int64?`

### DeviantArtFs.Api.Stash.Update
* AsyncExecute `IDeviantArtAccessToken token` `Int64 stackid` `IEnumerable<UpdateField> updates` -> `AsyncSeq<DeviantArtSuccessOrErrorResponse>`
* ExecuteAsync `IDeviantArtAccessToken token` `Int64 stackid` `IEnumerable<UpdateField> updates` -> `Task<DeviantArtSuccessOrErrorResponse>`

**UpdateField** (Title | Description | ClearDescription)


### DeviantArtFs.Api.User.dAmnToken
* AsyncExecute `IDeviantArtAccessToken token` -> `AsyncSeq<dAmnTokenResponse>`
* ExecuteAsync `IDeviantArtAccessToken token` -> `Task<dAmnTokenResponse>`

### DeviantArtFs.Api.User.Friends
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `FriendsRequest req` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtPagedResult<DeviantArtFriendRecord>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `FriendsRequest req` `DeviantArtPagingParams paging` -> `Task<DeviantArtPagedResult<DeviantArtFriendRecord>>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `FriendsRequest req` `Int32 offset` -> `AsyncSeq<DeviantArtFriendRecord>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `FriendsRequest req` `Int32 offset` `Int32 limit` -> `Task<DeviantArtFriendRecord[]>`

**FriendsRequest:**

* Username: `String`

### DeviantArtFs.Api.User.FriendsSearch
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `FriendsSearchRequest req` -> `AsyncSeq<DeviantArtListOnlyResponse<DeviantArtUser>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `FriendsSearchRequest req` -> `Task<DeviantArtListOnlyResponse<DeviantArtUser>>`

**FriendsSearchRequest:**

* Query: `String`
* Username: `String`

### DeviantArtFs.Api.User.FriendsUnwatch
* AsyncExecute `IDeviantArtAccessToken token` `String username` -> `AsyncSeq<DeviantArtSuccessOrErrorResponse>`
* ExecuteAsync `IDeviantArtAccessToken token` `String username` -> `Task<DeviantArtSuccessOrErrorResponse>`

### DeviantArtFs.Api.User.FriendsWatch
* AsyncExecute `IDeviantArtAccessToken token` `FriendsWatchRequest ps` -> `AsyncSeq<DeviantArtSuccessOrErrorResponse>`
* ExecuteAsync `IDeviantArtAccessToken token` `FriendsWatchRequest ps` -> `Task<DeviantArtSuccessOrErrorResponse>`

**FriendsWatchRequest:**

* Username: `String`
* Friend: `Boolean`
* Deviations: `Boolean`
* Journals: `Boolean`
* ForumThreads: `Boolean`
* Critiques: `Boolean`
* Scraps: `Boolean`
* Activity: `Boolean`
* Collections: `Boolean`

### DeviantArtFs.Api.User.FriendsWatching
* AsyncExecute `IDeviantArtAccessToken token` `String username` -> `AsyncSeq<FriendsWatchingResponse>`
* ExecuteAsync `IDeviantArtAccessToken token` `String username` -> `Task<FriendsWatchingResponse>`

### DeviantArtFs.Api.User.ProfileByName
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `ProfileByNameRequest req` -> `AsyncSeq<DeviantArtProfile>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `ProfileByNameRequest req` -> `Task<DeviantArtProfile>`

**ProfileByNameRequest:**

* Username: `String`
* ExtCollections: `Boolean`
* ExtGalleries: `Boolean`

### DeviantArtFs.Api.User.ProfileUpdate
* AsyncExecute `IDeviantArtAccessToken token` `IEnumerable<ProfileUpdateField> updates` -> `AsyncSeq<DeviantArtSuccessOrErrorResponse>`
* ExecuteAsync `IDeviantArtAccessToken token` `IEnumerable<ProfileUpdateField> updates` -> `Task<DeviantArtSuccessOrErrorResponse>`

**ProfileUpdateField** (UserIsArtist | ArtistLevel | ArtistSpecialty | Tagline | Countryid | Website)


### DeviantArtFs.Api.User.StatusById
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Guid id` -> `AsyncSeq<DeviantArtStatus>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `Guid id` -> `Task<DeviantArtStatus>`

### DeviantArtFs.Api.User.StatusesList
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `String username` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtPagedResult<DeviantArtStatus>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `String username` `DeviantArtPagingParams paging` -> `Task<DeviantArtPagedResult<DeviantArtStatus>>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `String username` `Int32 offset` -> `AsyncSeq<DeviantArtStatus>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `String username` `Int32 offset` `Int32 limit` -> `Task<DeviantArtStatus[]>`

### DeviantArtFs.Api.User.StatusPost
* AsyncExecute `IDeviantArtAccessToken token` `StatusPostRequest ps` -> `AsyncSeq<StatusPostResponse>`
* ExecuteAsync `IDeviantArtAccessToken token` `StatusPostRequest ps` -> `Task<StatusPostResponse>`

**StatusPostRequest:**

* Body: `String`
* Statusid: `Guid?`
* Parentid: `Guid?`
* Stashid: `Int64?`

### DeviantArtFs.Api.User.Watchers
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `FriendsRequest req` `DeviantArtPagingParams paging` -> `AsyncSeq<DeviantArtPagedResult<DeviantArtWatcherRecord>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `FriendsRequest req` `DeviantArtPagingParams paging` -> `Task<DeviantArtPagedResult<DeviantArtWatcherRecord>>`
* ToAsyncSeq `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `FriendsRequest req` `Int32 offset` -> `AsyncSeq<DeviantArtWatcherRecord>`
* ToArrayAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `FriendsRequest req` `Int32 offset` `Int32 limit` -> `Task<DeviantArtWatcherRecord[]>`

**FriendsRequest:**

* Username: `String`

### DeviantArtFs.Api.User.Whoami
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` -> `AsyncSeq<DeviantArtUser>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` -> `Task<DeviantArtUser>`

### DeviantArtFs.Api.User.Whois
* AsyncExecute `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `IEnumerable<String> usernames` -> `AsyncSeq<DeviantArtListOnlyResponse<DeviantArtUser>>`
* ExecuteAsync `IDeviantArtAccessToken token` `DeviantArtCommonParams common` `IEnumerable<String> usernames` -> `Task<DeviantArtListOnlyResponse<DeviantArtUser>>`

**String:**

* Chars: `Char`
* Length: `Int32`

### DeviantArtFs.Api.Util.Placebo
* AsyncIsValid `IDeviantArtAccessToken token` -> `AsyncSeq<Boolean>`
* IsValidAsync `IDeviantArtAccessToken token` -> `Task<Boolean>`

### DeviantArtFs.DeviantArtAuth
* AsyncGetToken `DeviantArtApp app` `String code` `Uri redirect_uri` -> `AsyncSeq<DeviantArtTokenResponse>`
* AsyncRefresh `DeviantArtApp app` `String refresh_token` -> `AsyncSeq<DeviantArtTokenResponse>`
* AsyncRevoke `String token` `Boolean revoke_refresh_only` -> `AsyncSeq<Unit>`
* RevokeAsync `String token` `Boolean revoke_refresh_only` -> `Task`
* GetTokenAsync `DeviantArtApp app` `String code` `Uri redirect_uri` -> `Task<DeviantArtTokenResponse>`
* RefreshAsync `DeviantArtApp app` `String refresh_token` -> `Task<DeviantArtTokenResponse>`

**DeviantArtApp:**

* client_id: `String`
* client_secret: `String`

### DeviantArtFs.DeviantArtRequest
* AsyncReadJson -> `AsyncSeq<String>`

