namespace DeviantArtFs

[<RequireQualifiedAccess>]
type FieldChange<'a> =
    | UpdateToValue of 'a
    | NoChange

module internal fch =
    let map f o =
        match o with
        | FieldChange.UpdateToValue s -> FieldChange.UpdateToValue (f s)
        | FieldChange.NoChange -> FieldChange.NoChange

    let toQuery (name: string) (value: FieldChange<'a>) = seq {
        match value with
        | FieldChange.UpdateToValue s ->
            if obj.ReferenceEquals(s, null) then
                failwithf "Null is not allowed (parameter: %s)" name
            else
                let str = s.ToString()
                yield sprintf "%s=%s" (dafs.urlEncode name) (dafs.urlEncode str)
        | FieldChange.NoChange -> ()
    }

[<RequireQualifiedAccess>]
type NullableStringFieldChange =
    | UpdateToValue of string
    | NoChange

module internal nsfch =
    let map f o =
        match o with
        | NullableStringFieldChange.UpdateToValue s -> NullableStringFieldChange.UpdateToValue (f s)
        | NullableStringFieldChange.NoChange -> NullableStringFieldChange.NoChange

    let rec toQuery (name: string) (value: NullableStringFieldChange) =
        match value with
        | NullableStringFieldChange.UpdateToValue "null" -> failwithf "The string \"null\" is not allowed (parameter: %s)" name
        | NullableStringFieldChange.UpdateToValue null -> fch.toQuery name (FieldChange.UpdateToValue "null")
        | NullableStringFieldChange.UpdateToValue v -> fch.toQuery name (FieldChange.UpdateToValue v)
        | NullableStringFieldChange.NoChange -> Seq.empty