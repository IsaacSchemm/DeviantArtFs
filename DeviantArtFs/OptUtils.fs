namespace DeviantArtFs

open System

module OptUtils =
    let boolDefault (o: bool option) =
        Option.toNullable o

    let intDefault (o: int option) =
        Option.toNullable o

    let longDefault (o: int64 option) =
        Option.toNullable o

    let guidDefault (o: Guid option) =
        Option.toNullable o

    let timeDefault (o: DateTimeOffset option) =
        Option.toNullable o

    let stringDefault (o: string option) =
        match o with
        | Some s -> s
        | None -> ""

    let recordDefault (o: 'a option) =
        match o with
        | Some s -> Seq.singleton s
        | None -> Seq.empty

    let listDefault (o: 'a list option) =
        match o with
        | Some s -> s
        | None -> List.empty

    let mapDefault (o: Map<'a, 'b> option) =
        match o with
        | Some s -> s
        | None -> Map.empty

    let toObjSeq (o: 'a option) =
        match o with
        | Some s -> Seq.singleton (s :> obj)
        | None -> Seq.empty