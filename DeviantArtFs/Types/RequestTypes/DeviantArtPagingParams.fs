﻿namespace DeviantArtFs

open System

type IDeviantArtPagingParams =
    abstract member Offset: int
    abstract member Limit: Nullable<int>

type DeviantArtPagingParams() =
    member val Offset = 0 with get, set
    member val Limit = Nullable<int>() with get, set
    interface IDeviantArtPagingParams with
        member this.Offset = this.Offset
        member this.Limit = this.Limit