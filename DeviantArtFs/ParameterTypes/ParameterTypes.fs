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

type StashStack = StashStack of int64 | RootStack

module StashStack =
    let id stack = match stack with | StashStack x -> x | RootStack -> 0L

type StashItem = StashItem of int64

module StashItem =
    let id item = match item with | StashItem x -> x

type StashDeltaCursor = StashDeltaCursor of string | InitialStashDeltaRequest

type MatureLevel = MatureStrict | MatureModerate
type MatureClassification = Nudity | Sexual | Gore | Language | Ideology
type Maturity = Mature of MatureLevel * MatureClassification Set | NotMature
with static member MatureBecause level classifications = Mature (level, Set.ofSeq classifications)

type DisplayResolution = Original=0 | Max400Px=1 | Max600px=2 | Max800px=3 | Max900px=4 | Max1024px=5 | Max1280px=6 | Max1600px=7 | Max1920px=8

type Sharing = AllowSharing | HideShareButtons | HideShareButtonsAndMembersOnly

module CreativeCommons =
    type AttributionClause = Attribution
    type CommercialUseClause = NonCommercial
    type DerivativeWorksClause = NoDerivatives | ShareAlike

    type License = AttributionClause * CommercialUseClause option * DerivativeWorksClause option

    module LicenseBuilder =
        type Clause = BY | NC | ND | SA

        let Build clauses =
            let att = if Seq.contains BY clauses then Attribution else failwith "The attribution clause (BY) must be included"
            let comm = if Seq.contains NC clauses then Some NonCommercial else None
            let deriv = if Seq.contains ND clauses then Some NoDerivatives else if Seq.contains SA clauses then Some ShareAlike else None
            License (att, comm, deriv)

type License = CreativeCommonsLicense of CreativeCommons.License | DefaultLicense

type GallerySet = GallerySet of Guid Set
with
    static member Create x = GallerySet (Set.ofSeq x)
    static member Empty = GallerySet Set.empty

type PublishParameters = {
    maturity: Maturity
    submissionPolicyAgreement: bool
    termsOfServiceAgreement: bool
    featured: bool
    allowComments: bool
    requestCritique: bool
    displayResolution: DisplayResolution
    sharing: Sharing
    license: License
    destinations: GallerySet
    allowFreeDownload: bool
    addWatermark: bool
} with
    static member CreateSet x = Set.ofSeq x
    static member Default = {
        maturity = NotMature
        submissionPolicyAgreement = false
        termsOfServiceAgreement = false
        featured = true
        allowComments = true
        requestCritique = false
        displayResolution = DisplayResolution.Original
        sharing = AllowSharing
        license = DefaultLicense
        destinations = GallerySet.Empty
        allowFreeDownload = false
        addWatermark = false
    }

type StackModification = ModifyStackTitle of string | ModifyStackDescription of string | ClearStackDescription

type AdditionalUser = AdditionalUser of string | NoAdditionalUser

type WatchType = MakeFriend | WatchDeviations | WatchJournals | WatchForumThreads | WatchCritiques | WatchScraps | WatchActivity | WatchCollections
with
    static member None = Set.empty<WatchType>
    static member All = Set.ofList [MakeFriend; WatchDeviations; WatchJournals; WatchForumThreads; WatchCritiques; WatchScraps; WatchActivity; WatchCollections]

type ProfileExtParams = ExtCollections | ExtGalleries
with
    static member None = Set.empty<ProfileExtParams>
    static member All = Set.ofList [ExtCollections; ExtGalleries]