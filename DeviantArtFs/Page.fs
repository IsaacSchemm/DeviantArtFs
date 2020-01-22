namespace DeviantArtFs

module internal Page =
    let map f (this: DeviantArtPagedResult<'a>) = {
        has_more = this.has_more
        next_offset = this.next_offset
        estimated_total = this.estimated_total
        has_less = this.has_less
        prev_offset = this.prev_offset
        name = this.name
        results = List.map f this.results
    }

    let wrap (a: DeviantArtPagedResult<'a>) =
        a :> IBclDeviantArtPagedResult<'a>

    let mapAndWrap f a = a |> map f |> wrap