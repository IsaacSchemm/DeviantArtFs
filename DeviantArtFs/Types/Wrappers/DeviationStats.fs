namespace DeviantArtFs

[<AllowNullLiteral>]
type IDeviationStats =
    abstract member Comments: int
    abstract member Favourites: int

type DeviationStats = {
    comments: int
    favourites: int
} with
    interface IDeviationStats with
        member this.Comments = this.comments
        member this.Favourites = this.favourites