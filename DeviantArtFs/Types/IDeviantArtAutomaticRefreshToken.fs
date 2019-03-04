namespace DeviantArtFs

open System.Threading.Tasks

type IDeviantArtAuth =
    abstract member RefreshAsync: string -> Task<IDeviantArtRefreshToken>

type IDeviantArtAutomaticRefreshToken =
    inherit IDeviantArtRefreshToken
    abstract member DeviantArtAuth: IDeviantArtAuth with get
    abstract member UpdateTokenAsync: IDeviantArtRefreshToken -> Task