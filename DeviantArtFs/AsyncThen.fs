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

    let mapPagedResult f a = async {
        let! o = a
        let r = o :> IBclDeviantArtPagedResult<'a>
        return {
            new IBclDeviantArtPagedResult<'b> with
                member __.HasMore = r.HasMore
                member __.NextOffset = r.NextOffset
                member __.HasLess = r.HasLess
                member __.PrevOffset = r.PrevOffset
                member __.EstimatedTotal = r.EstimatedTotal
                member __.Name = r.Name
                member __.Results = Seq.map f r.Results
        }
    }

    let mapFeedCursorResult f a = async {
        let! o = a
        let r = o :> IBclDeviantArtFeedCursorResult<'a>
        return {
            new IBclDeviantArtFeedCursorResult<'b> with
                member __.HasMore = r.HasMore
                member __.Cursor = r.Cursor
                member __.Items = Seq.map f r.Items
        }
    }

    let mapMessageCursorResult f a = async {
        let! o = a
        let r = o :> IBclDeviantArtMessageCursorResult<'a>
        return {
            new IBclDeviantArtMessageCursorResult<'b> with
                member __.HasMore = r.HasMore
                member __.Cursor = r.Cursor
                member __.Results = Seq.map f r.Results
        }
    }