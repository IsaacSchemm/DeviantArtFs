namespace DeviantArtFs.ParameterTypes

open System

type DailyDeviationDate = DailyDeviationsToday | DailyDeviationsFor of DateTime
with static member Default = DailyDeviationsToday

type SearchQueryParameter = NoSearchQuery | SearchQuery of string
with static member Default = NoSearchQuery

type PopularTimeRange = UnspecifiedPopularTimeRange | PopularNow | PopularOneWeek | PopularOneMonth | PopularAllTime
with static member Default = UnspecifiedPopularTimeRange

type UserJournalFilter = NoUserJournalFilter | FeaturedJournalsOnly
with static member Default = FeaturedJournalsOnly

type CollectionsUser = CollectionsForCurrentUser | CollectionsForUser of string
with static member Default = CollectionsForCurrentUser

type CollectionsFolderCalculateSize = CollectionsFolderCalculateSize of bool
with static member Default = CollectionsFolderCalculateSize false

type CollectionsFolderPreload = CollectionsFolderPreload of bool
with static member Default = CollectionsFolderPreload false