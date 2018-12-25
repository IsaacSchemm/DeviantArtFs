namespace DeviantArtFs.Stash.Marshal

type IStashRoot =
    abstract member Nodes: seq<IStashNode>