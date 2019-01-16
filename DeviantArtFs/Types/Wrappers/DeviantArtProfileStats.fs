namespace DeviantArtFs

type IBclDeviantArtProfileStats =
    abstract member UserDeviations: int
    abstract member UserFavourites: int
    abstract member UserComments: int
    abstract member ProfilePageviews: int
    abstract member ProfileComments: int

type DeviantArtProfileStats = {
    UserDeviations: int
    UserFavourites: int
    UserComments: int
    ProfilePageviews: int
    ProfileComments: int
} with
    interface IBclDeviantArtProfileStats with
        member this.ProfileComments = this.ProfileComments
        member this.ProfilePageviews = this.ProfilePageviews
        member this.UserComments = this.UserComments
        member this.UserDeviations = this.UserDeviations
        member this.UserFavourites = this.UserFavourites