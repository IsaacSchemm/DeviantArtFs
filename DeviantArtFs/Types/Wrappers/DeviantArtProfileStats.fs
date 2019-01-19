namespace DeviantArtFs

type IBclDeviantArtProfileStats =
    abstract member UserDeviations: int
    abstract member UserFavourites: int
    abstract member UserComments: int
    abstract member ProfilePageviews: int
    abstract member ProfileComments: int

type DeviantArtProfileStats = {
    user_deviations: int
    user_favourites: int
    user_comments: int
    profile_pageviews: int
    profile_comments: int
} with
    interface IBclDeviantArtProfileStats with
        member this.ProfileComments = this.profile_comments
        member this.ProfilePageviews = this.profile_pageviews
        member this.UserComments = this.user_comments
        member this.UserDeviations = this.user_deviations
        member this.UserFavourites = this.user_favourites