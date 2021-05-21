namespace DeviantArtFs.ParameterTypes

open System

type PagingOffset = FromStart | PagingOffset of int
with static member Default = FromStart

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

type MessageFolder = MessageFolder of Guid | Inbox
with static member Default = Inbox

type MessageMode = StackedMessageMode | FlatMessageMode
with static member Default = StackedMessageMode

type MessageCursor = MessageCursor of string | StartingCursor
with static member Default = StartingCursor

type MessageDeletionTarget = DeleteMessage of string | DeleteStack of string

type FeedbackMessageType = Comments | Replies | Activity