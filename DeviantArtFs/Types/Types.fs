namespace DeviantArtFs

open System
open System.Net
open FSharp.Data

type IDeviantArtAccessToken =
    abstract member AccessToken: string with get

type IDeviantArtRefreshToken =
    inherit IDeviantArtAccessToken
    abstract member ExpiresAt: DateTimeOffset with get
    abstract member RefreshToken: string with get

type DeviantArtException(resp: WebResponse, body: DeviantArtErrorResponse.Root) =
    inherit Exception(body.ErrorDescription)

    member __.ResponseBody = body
    member __.StatusCode =
        match resp with
        | :? HttpWebResponse as h -> Nullable h.StatusCode
        | _ -> Nullable()

type ExtParams =
    struct
        val ExtSubmission: bool
        val ExtCamera: bool
        val ExtStats: bool
    end

type ArtistLevel =
    | Student=1
    | Hobbyist=2
    | Professional=3
type ArtistSpecialty =
    | ArtisanCrafts = 1
    | DesignAndInterfaces = 2
    | DigitalArt = 3
    | FilmAndAnimation = 4
    | Literature = 5
    | Photography = 6
    | TraditionalArt = 7
    | Other = 8
    | Varied = 9

[<AllowNullLiteral>]
type IDeviantArtUser =
    abstract member Userid: Guid
    abstract member Username: string
    abstract member Usericon: string
    abstract member Type: string

type IDeviantArtCollection =
    abstract member Folderid: Guid
    abstract member Name: string

type IDeviantArtFolder =
    abstract member Folderid: Guid
    abstract member Parent: Nullable<Guid>
    abstract member Name: string

type IDeltaEntry =
    abstract member Itemid: Nullable<int64>
    abstract member Stackid: Nullable<int64>
    abstract member Metadata: string
    abstract member Position: Nullable<int>