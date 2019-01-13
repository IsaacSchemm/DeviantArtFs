namespace DeviantArtFs

module internal iop =
    let thenTo f a = async {
        let! o = a
        return f o
    }

    let thenMap f a = async {
        let! o = a
        return Seq.map f o
    }

    let thenMapResult f a = async {
        let! o = a
        let r = o :> IDeviantArtPagedResult<'a>
        return {
            new IDeviantArtPagedResult<'b> with
                member __.HasMore = r.HasMore
                member __.NextOffset = r.NextOffset
                member __.HasLess = r.HasLess
                member __.PrevOffset = r.PrevOffset
                member __.EstimatedTotal = r.EstimatedTotal
                member __.Name = r.Name
                member __.Results = Seq.map f r.Results
        }
    }

    let thenCastResult a = thenMapResult id a