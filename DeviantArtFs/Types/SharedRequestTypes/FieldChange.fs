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

    let allowNull (value: FieldChange<string>) =
        match value with
        | FieldChange.UpdateToValue "null" -> failwithf "The string \"null\" is not allowed here"
        | FieldChange.UpdateToValue null -> FieldChange.UpdateToValue "null"
        | _ -> value