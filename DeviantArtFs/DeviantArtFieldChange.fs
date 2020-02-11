namespace DeviantArtFs

/// A discrminated union that specifies whether to make a change to a field,
/// and (if so) what to change it to.
[<RequireQualifiedAccess>]
type DeviantArtFieldChange<'a> =
    | UpdateToValue of 'a
    | NoChange

module internal fch =
    let map f o =
        match o with
        | DeviantArtFieldChange.UpdateToValue s -> DeviantArtFieldChange.UpdateToValue (f s)
        | DeviantArtFieldChange.NoChange -> DeviantArtFieldChange.NoChange

    /// Modify a DeviantArtFieldChange union for fields on DeviantArt that
    /// apply a null value if the string "null" is given.
    let allowNull (value: DeviantArtFieldChange<string>) =
        match value with
        | DeviantArtFieldChange.UpdateToValue "null" -> failwithf "The string \"null\" is not allowed here"
        | DeviantArtFieldChange.UpdateToValue null -> DeviantArtFieldChange.UpdateToValue "null"
        | _ -> value