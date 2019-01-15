namespace DeviantArtFs

open DeviantArtFs

type IBclDeviantArtProfile =
    abstract member User: IDeviantArtUser
    abstract member IsWatching: bool
    abstract member ProfileUrl: string
    abstract member UserIsArtist: bool
    abstract member ArtistLevel: string
    abstract member ArtistSpecialty: string
    abstract member RealName: string
    abstract member Tagline: string
    abstract member Countryid: int
    abstract member Country: string
    abstract member Website: string
    abstract member Bio: string
    abstract member CoverPhoto: string
    abstract member ProfilePic: IBclDeviation
    abstract member LastStatus: IBclDeviantArtStatus
    abstract member Stats: IDeviantArtProfileStats
    abstract member Collections: seq<IBclDeviantArtCollectionFolder>
    abstract member Galleries: seq<IBclDeviantArtGalleryFolder>

type DeviantArtProfile(original: ProfileResponse.Root) =
    member __.User = {
        new IDeviantArtUser with
            member __.Userid = original.User.Userid
            member __.Username = original.User.Username
            member __.Usericon = original.User.Usericon
            member __.Type = original.User.Type
    }

    member __.IsWatching = original.IsWatching
    member __.ProfileUrl = original.ProfileUrl
    member __.UserIsArtist = original.UserIsArtist
    member __.ArtistLevel = original.ArtistLevel
    member __.ArtistSpecialty = original.ArtistSpecialty
    member __.RealName = original.RealName
    member __.Tagline = original.Tagline
    member __.Countryid = original.Countryid
    member __.Country = original.Country
    member __.Website = original.Website
    member __.Bio = original.Bio
    member __.CoverPhoto = original.CoverPhoto

    member __.ProfilePic =
        original.ProfilePic
        |> Option.map (fun x -> x.JsonValue.ToString())
        |> Option.map Deviation.Parse
    member __.LastStatus =
        original.LastStatus
        |> Option.map (fun x -> x.JsonValue.ToString())
        |> Option.map DeviantArtStatus.Parse

    member __.Stats = {
        new IDeviantArtProfileStats with
            member __.UserDeviations = original.Stats.UserDeviations
            member __.UserFavourites = original.Stats.UserFavourites
            member __.UserComments = original.Stats.UserComments
            member __.ProfilePageviews = original.Stats.ProfilePageviews
            member __.ProfileComments = original.Stats.ProfileComments
    }

    member __.Collections =
        original.Collections
        |> Seq.map (fun g -> g.JsonValue.ToString())
        |> Seq.map DeviantArtCollectionFolder.Parse

    member __.Galleries =
        original.Galleries
        |> Seq.map (fun g -> g.JsonValue.ToString())
        |> Seq.map DeviantArtGalleryFolder.Parse

    interface IBclDeviantArtProfile with
        member this.ArtistLevel = this.ArtistLevel
        member this.ArtistSpecialty = this.ArtistSpecialty |> Option.toObj
        member this.Bio = this.Bio
        member this.Collections = this.Collections |> Seq.map (fun f -> f :> IBclDeviantArtCollectionFolder)
        member this.Country = this.Country
        member this.Countryid = this.Countryid
        member this.CoverPhoto = this.CoverPhoto |> Option.toObj
        member this.Galleries = this.Galleries |> Seq.map (fun f -> f :> IBclDeviantArtGalleryFolder)
        member this.IsWatching = this.IsWatching
        member this.LastStatus = this.LastStatus |> Option.map DeviantArtStatus.MapToBclInterface |> Option.toObj
        member this.ProfilePic = this.ProfilePic |> Option.map (fun s -> s :> IBclDeviation) |> Option.toObj
        member this.ProfileUrl = this.ProfileUrl
        member this.RealName = this.RealName
        member this.Stats = this.Stats
        member this.Tagline = this.Tagline
        member this.User = this.User
        member this.UserIsArtist = this.UserIsArtist
        member this.Website = this.Website