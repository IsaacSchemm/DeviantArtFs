namespace DeviantArtFs.ParameterTypes

open System

type PagingOffset = StartingOffset | PagingOffset of int
with static member Default = StartingOffset

type PagingLimit = PagingLimit of int | MaximumPagingLimit | DefaultPagingLimit
with static member Default = DefaultPagingLimit

type ObjectExpansion = StatusFullText | UserDetails | UserGeo | UserProfile | UserStats | UserWatch
with
    static member All = [StatusFullText; UserDetails; UserGeo; UserProfile; UserStats; UserWatch]
    static member None = List.empty<ObjectExpansion>

type ExtParams = ExtSubmission | ExtCamera | ExtStats | ExtCollection
with
    static member All = [ExtSubmission; ExtCamera; ExtStats; ExtCollection]
    static member None = List.empty<ExtParams>

type UserScope = ForCurrentUser | ForUser of string
with static member Default = ForCurrentUser

type CalculateSize = CalculateSize of bool
with static member Default = CalculateSize false

type FolderPreload = FolderPreload of bool
with static member Default = FolderPreload false

type DailyDeviationDate = DailyDeviationsToday | DailyDeviationsFor of DateTime
with static member Default = DailyDeviationsToday

type SearchQuery = NoSearchQuery | SearchQuery of string
with static member Default = NoSearchQuery

type PopularTimeRange = UnspecifiedPopularTimeRange | PopularNow | PopularOneWeek | PopularOneMonth | PopularAllTime
with static member Default = UnspecifiedPopularTimeRange

type UserJournalFilter = NoUserJournalFilter | FeaturedJournalsOnly
with static member Default = FeaturedJournalsOnly

type CommentSubject = OnDeviation of Guid | OnProfile of string | OnStatus of Guid

type CommentReplyType = DirectReply | InReplyToComment of Guid
with static member Default = DirectReply

type CommentDepth = CommentDepth of int
with
    static member Default = CommentDepth 0
    static member Max = CommentDepth 5

type IncludeRelatedItem = IncludeRelatedItem of bool

type EmbeddedDeviationOffset = StartWithFirstEmbeddedDeviation | StartWithEmbeddedDeviation of Guid
with static member Default = StartWithFirstEmbeddedDeviation

type GalleryFolderScope = SingleGalleryFolder of Guid | AllGalleryFoldersNewest | AllGalleryFoldersPopular

type MessageFolder = Inbox | MessageFolder of Guid
with static member Default = Inbox

type StackMessages = StackMessages of bool
with static member Default = StackMessages true

type MessageCursor = StartingCursor | MessageCursor of string
with static member Default = StartingCursor

type MessageDeletionTarget = DeleteMessage of string | DeleteStack of string

type FeedbackMessageType = CommentFeedbackMessages | ReplyFeedbackMessages | ActivityFeedbackMessages