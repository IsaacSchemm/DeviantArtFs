namespace DeviantArtFs

module internal OptUtils =
    let toSeq (o: 'a option) =
        match o with
        | Some s -> Seq.singleton s
        | None -> Seq.empty

    let toObjSeq (o: 'a option) =
        match o with
        | Some s -> Seq.singleton (s :> obj)
        | None -> Seq.empty

    let emptyIfNone (o: 'a list option) =
        match o with
        | Some s -> s
        | None -> List.empty