namespace DeviantArtFs

[<AllowNullLiteral>]
type IBclDeviationStats =
    abstract member Comments: int
    abstract member Favourites: int

type DeviationStats = {
    comments: int
    favourites: int
} with
    interface IBclDeviationStats with
        member this.Comments = this.comments
        member this.Favourites = this.favourites