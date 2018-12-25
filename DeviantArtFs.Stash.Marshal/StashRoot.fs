namespace DeviantArtFs.Stash.Marshal

type StashRoot() =
    let nodes = new ResizeArray<IStashNode>()

    interface IStashRoot with
        member __.Nodes = nodes :> seq<IStashNode>