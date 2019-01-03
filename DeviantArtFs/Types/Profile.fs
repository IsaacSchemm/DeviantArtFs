namespace DeviantArtFs

type IProfileStats =
    abstract member UserDeviations: int
    abstract member UserFavourites: int
    abstract member UserComments: int
    abstract member ProfilePageviews: int
    abstract member ProfileComments: int

type Profile(original: ProfileResponse.Root) =
    let profilePic =
        original.ProfilePic
        |> Option.map (fun x -> x.JsonValue.ToString())
        |> Option.map DeviationResponse.Parse
        |> Option.map Deviation
    let lastStatus =
        original.LastStatus
        |> Option.map (fun x -> x.JsonValue.ToString())
        |> Option.map StatusResponse.Parse
        |> Option.map Status

    member __.Original = original

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
    member __.ArtistSpecialty = original.ArtistSpecialty |> Option.toObj
    member __.RealName = original.RealName
    member __.Tagline = original.Tagline
    member __.Countryid = original.Countryid
    member __.Country = original.Country
    member __.Website = original.Website
    member __.Bio = original.Bio
    member __.CoverPhoto = original.CoverPhoto |> Option.toObj

    member __.ProfilePic = profilePic |> Option.toObj
    member __.LastStatus = lastStatus |> Option.toObj

    member __.Stats = {
        new IProfileStats with
            member __.UserDeviations = original.Stats.UserDeviations
            member __.UserFavourites = original.Stats.UserFavourites
            member __.UserComments = original.Stats.UserComments
            member __.ProfilePageviews = original.Stats.ProfilePageviews
            member __.ProfileComments = original.Stats.ProfileComments
    }

    member __.Collections =
        original.Collections
        |> Seq.map (fun c -> {
            new IDeviantArtCollection with
                member __.Folderid = c.Folderid
                member __.Name = c.Name
        })

    member __.Galleries =
        original.Galleries
        |> Seq.map (fun g -> {
            new IDeviantArtFolder with
                member __.Folderid = g.Folderid
                member __.Parent = g.Parent |> Option.toNullable
                member __.Name = g.Name
        })