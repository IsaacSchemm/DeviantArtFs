namespace DeviantArtFs

module internal Page =
    let map f (this: DeviantArtPagedResult<'a>) = {
        has_more = this.has_more
        next_offset = this.next_offset
        results = List.map f this.results
    }