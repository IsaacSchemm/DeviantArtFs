namespace DeviantArtFs

module OptUtils =
    let toNullable (o: 'a option) =
        Option.toNullable o

    let stringDefault (o: string option) =
        match o with
        | Some s -> s
        | None -> ""

    let listDefault (o: 'a list option) =
        match o with
        | Some s -> s
        | None -> List.empty

    let mapDefault (o: Map<'a, 'b> option) =
        match o with
        | Some s -> s
        | None -> Map.empty

    let recordDefault (o: 'a option) =
        match o with
        | Some s -> Seq.singleton s
        | None -> Seq.empty

    let toObjSeq (o: 'a option) =
        match o with
        | Some s -> Seq.singleton (s :> obj)
        | None -> Seq.empty