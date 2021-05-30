namespace DeviantArtFs.ResponseTypes

type Topic = {
    name: string
    canonical_name: string
    example_deviations: Deviation list option
    deviations: Deviation list option
} with
    member this.ExampleDeviations = this.example_deviations |> Option.defaultValue List.empty
    member this.Deviations = this.deviations |> Option.defaultValue List.empty