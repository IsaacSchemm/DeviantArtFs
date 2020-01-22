namespace DeviantArtFs

module internal AsyncThen =
    let map f a = async {
        let! o = a
        return f o
    }

    let mapSeq f a = async {
        let! o = a
        return Seq.map f o
    }

    let mapPage f (a: Async<DeviantArtPagedResult<'a>>) = async {
        let! o = a
        return o.Map f
    }

    let mapAndWrapPage f (a: Async<DeviantArtPagedResult<'a>>) = async {
        let! o = a
        return o.Map f :> IBclDeviantArtPagedResult<'b>
    }