namespace DeviantArtFs.ParameterTypes

type CollectionsUser = CollectionsForCurrentUser | CollectionsForUser of string
with static member Default = CollectionsForCurrentUser

type CollectionsFolderCalculateSize = CollectionsFolderCalculateSize of bool
with static member Default = CollectionsFolderCalculateSize false

type CollectionsFolderPreload = CollectionsFolderPreload of bool
with static member Default = CollectionsFolderPreload false