﻿namespace DeviantArtFs.ParameterTypes

open System
open DeviantArtFs

type PagingOffset = StartingOffset | PagingOffset of int
with static member Default = StartingOffset

type PagingLimit = PagingLimit of int | MaximumPagingLimit | DefaultPagingLimit
with static member Default = DefaultPagingLimit

type UserScope = ForCurrentUser | ForUser of string
with static member Default = ForCurrentUser

type CalculateSize = CalculateSize of bool
with static member Default = CalculateSize false

type FolderPreload = FolderPreload of bool
with static member Default = FolderPreload false

type FilterEmptyFolder = FilterEmptyFolder of bool
with static member Default = FilterEmptyFolder false

[<RequireQualifiedAccess>]
type FolderUpdateType = Name of string | Description of string | CoverDeviationId of Guid

type GalleryFolderScope = SingleGalleryFolder of Guid | AllGalleryFoldersNewest | AllGalleryFoldersPopular

type MatureLevel = MatureStrict | MatureModerate

type MatureClassification = Nudity | Sexual | Gore | Language | Ideology

type Maturity = Mature of MatureLevel * Set<MatureClassification> | NotMature
with
    static member MatureBecause l c = Mature (l, Set.ofSeq c)

type Commerical = CommericalYes | CommericalNo
type Modify = ModifyYes | ModifyNo | ModifyShare

type RestrictionSet = {
    commercial: Commerical
    modify: Modify
}

type License = CreativeCommons of RestrictionSet | DefaultLicense
with
    static member All = [
        DefaultLicense
        for c in [CommericalYes; CommericalNo] do
            for m in [ModifyYes; ModifyNo; ModifyShare] do
                CreativeCommons { commercial = c; modify = m }
    ]
    override this.ToString() =
        match this with
        | DefaultLicense -> "Default"
        | CreativeCommons restrictionSet ->
            let abbr = String.concat "-" [
                "BY"
                match restrictionSet.commercial with | CommericalYes -> () | CommericalNo -> "NC"
                match restrictionSet.modify with | ModifyYes -> () | ModifyNo -> "ND" | ModifyShare -> "SA"
            ]
            $"CC {abbr}"

module internal QueryFor =
    let optionalParameters ops = seq {
        for o in ops do
        match o with
        | OptionalParameter.Expansion s ->
            "expand", String.concat "," (seq {
                for x in s do
                    match x with
                    | Expansion.CommentFullText -> "comment.fulltext"
                    | Expansion.DeviationPinned -> "deviation.pinned"
                    | Expansion.DeviationFullText -> "deviation.fulltext"
                    | Expansion.StatusFullText -> "status.fulltext"
                    | Expansion.UserDetails -> "user.details"
                    | Expansion.UserGeo -> "user.geo"
                    | Expansion.UserProfile -> "user.profile"
                    | Expansion.UserStats -> "user.stats"
                    | Expansion.UserWatch -> "user.watch"
            })
        | OptionalParameter.ExtParam ExtParam.Submission -> "ext_submission", "1"
        | OptionalParameter.ExtParam ExtParam.Camera -> "ext_camera", "1"
        | OptionalParameter.ExtParam ExtParam.Stats -> "ext_stats", "1"
        | OptionalParameter.ExtParam ExtParam.Collection -> "ext_collection", "1"
        | OptionalParameter.ExtParam ExtParam.Gallery -> "ext_collection", "1"
        | OptionalParameter.MatureContent true -> "is_mature", "true"
        | OptionalParameter.MatureContent false -> "is_mature", "false"
        | OptionalParameter.CustomParameter (key, value) -> key, value
    }

    let offset offset = seq {
        match offset with
        | StartingOffset -> ()
        | PagingOffset o -> ("offset", string o)
    }

    let limit limit maximum = seq {
        match limit with
        | PagingLimit l -> "limit", string (min l maximum)
        | MaximumPagingLimit -> "limit", string maximum
        | DefaultPagingLimit -> ()
    }

    let userScope scope = seq {
        match scope with
        | ForUser s -> "username", s
        | ForCurrentUser -> ()
    }

    let calculateSize calculateSize = seq {
        match calculateSize with
        | CalculateSize true -> "calculate_size", "1"
        | CalculateSize false -> "calculate_size", "0"
    }

    let folderPreload extPreload = seq {
        match extPreload with
        | FolderPreload true -> "ext_preload", "1"
        | FolderPreload false -> "ext_preload", "0"
    }

    let filterEmptyFolder filterEmptyFolder = seq {
        match filterEmptyFolder with
        | FilterEmptyFolder true -> "filter_empty_folder", "1"
        | FilterEmptyFolder false -> "filter_empty_folder", "0"
    }

    let folderUpdates o = seq {
        for u in o do
            match u with
            | FolderUpdateType.Name x -> "name", x
            | FolderUpdateType.Description x -> "description", x
            | FolderUpdateType.CoverDeviationId x -> " cover_deviationid", string x
    }