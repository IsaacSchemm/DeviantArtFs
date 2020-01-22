namespace DeviantArtFs

module internal AsyncThen =
    let map f a = async {
        let! o = a
        return f o
    }

    let mapSeq f a =
        map (Seq.map f) a

    let mapAndWrapPage f (a: Async<DeviantArtPagedResult<'a>>) =
        map (Page.mapAndWrap f) a