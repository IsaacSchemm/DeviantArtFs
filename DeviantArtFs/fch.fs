namespace DeviantArtFs

module internal fch =
    let map f o =
        match o with
        | FieldChange.UpdateToValue s -> FieldChange.UpdateToValue (f s)
        | FieldChange.NoChange -> FieldChange.NoChange

    let allowNull (value: FieldChange<string>) =
        match value with
        | FieldChange.UpdateToValue "null" -> failwithf "The string \"null\" is not allowed here"
        | FieldChange.UpdateToValue null -> FieldChange.UpdateToValue "null"
        | _ -> value

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