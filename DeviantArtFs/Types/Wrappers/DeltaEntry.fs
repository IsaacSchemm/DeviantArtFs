namespace DeviantArtFs

open System

type IBclDeltaEntry =
    abstract member Itemid: Nullable<int64>
    abstract member Stackid: Nullable<int64>
    abstract member MetadataJson: string
    abstract member Position: Nullable<int>