namespace DeviantArtFs

[<RequireQualifiedAccess>]
type DeviantArtFieldChange<'a> =
    | UpdateToValue of 'a
    | NoChange

module internal fch =
    let map f o =
        match o with
        | DeviantArtFieldChange.UpdateToValue s -> DeviantArtFieldChange.UpdateToValue (f s)
        | DeviantArtFieldChange.NoChange -> DeviantArtFieldChange.NoChange

    let allowNull (value: DeviantArtFieldChange<string>) =
        match value with
        | DeviantArtFieldChange.UpdateToValue "null" -> failwithf "The string \"null\" is not allowed here"
        | DeviantArtFieldChange.UpdateToValue null -> DeviantArtFieldChange.UpdateToValue "null"
        | _ -> value