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

type PopularTimeRange =
| PopularTimeRangeUnspecified
| PopularNow
| PopularOneWeek
| PopularOneMonth
| PopularAllTime
with static member Default = PopularTimeRangeUnspecified

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

type MatureClassificationSet = MatureClassificationSet of MatureClassification Set
with
    static member Create x = MatureClassificationSet (Set.ofSeq x)
    static member Single x = MatureClassificationSet (Set.ofList [x])

type Maturity = Mature of MatureLevel * MatureClassificationSet | NotMature

type DisplayResolution =
| Original=0
| Max400Px=1
| Max600px=2
| Max800px=3
| Max900px=4
| Max1024px=5
| Max1280px=6
| Max1600px=7
| Max1920px=8

type Sharing = AllowSharing | HideShareButtons | HideShareButtonsAndMembersOnly

module CreativeCommons =
    type AttributionClause = Attribution
    type CommercialUseClause = NonCommercial
    type DerivativeWorksClause = NoDerivatives | ShareAlike

    let BY = Attribution
    let NC = Some NonCommercial
    let ND = Some NoDerivatives
    let SA = Some ShareAlike

    let NoCommercialUseClause: CommercialUseClause option = None
    let NoDerivativeWorksClause: DerivativeWorksClause option = None

    type License = AttributionClause * CommercialUseClause option * DerivativeWorksClause option

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

type StackModification =
| ModifyStackTitle of string
| ModifyStackDescription of string
| ClearStackDescription

type AdditionalUser = AdditionalUser of string | NoAdditionalUser

type WatchType =
| MakeFriend
| WatchDeviations
| WatchJournals
| WatchForumThreads
| WatchCritiques
| WatchScraps
| WatchActivity
| WatchCollections
with
    static member None = Set.empty<WatchType>
    static member All = Set.ofList [MakeFriend; WatchDeviations; WatchJournals; WatchForumThreads; WatchCritiques; WatchScraps; WatchActivity; WatchCollections]

type ProfileExtParams = ExtCollections | ExtGalleries
with
    static member None = Set.empty<ProfileExtParams>
    static member All = Set.ofList [ExtCollections; ExtGalleries]

type ArtistLevel =
| None=0
| Student=1
| Hobbyist=2
| Professional=3

type ArtistSpecialty =
| None=0
| ArtisanCrafts = 1
| DesignAndInterfaces = 2
| DigitalArt = 3
| FilmAndAnimation = 4
| Literature = 5
| Photography = 6
| TraditionalArt = 7
| Other = 8
| Varied = 9

type ProfileModification =
| UserIsArtist of bool
| ArtistLevel of ArtistLevel
| ArtistSpecialty of ArtistSpecialty
| Tagline of string
| Countryid of int
| Website of string

type EmbeddableObject = DeviationToEmbed of Guid | StatusToEmbed of Guid | NoEmbeddableObject
type EmbeddableObjectParent = EmbeddableObjectParentStatus of Guid | NoEmbeddableObjectParent
type EmbeddableStashItem = EmbeddableStashItem of Guid | NoEmbeddableStashItem

type EmbeddableStatusContent = {
    object: EmbeddableObject
    parent: EmbeddableObjectParent
    stash_item: EmbeddableStashItem
} with
    static member None = {
        object = NoEmbeddableObject
        parent = NoEmbeddableObjectParent
        stash_item = NoEmbeddableStashItem
    }