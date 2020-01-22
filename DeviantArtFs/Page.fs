namespace DeviantArtFs

module internal Page =
    let map f (this: DeviantArtPagedResult<'a>) = {
        has_more = this.has_more
        next_offset = this.next_offset
        results = List.map f this.results
    }

    let wrap (a: DeviantArtPagedResult<'a>) =
        a :> IBclDeviantArtPagedResult<'a>

    let mapAndWrap f a = a |> map f |> wrap