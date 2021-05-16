namespace DeviantArtFs.ParameterTypes

type PagingOffset = FromStart | PagingOffset of int
with static member Default = FromStart

type PagingLimit = PagingLimit of int | MaximumPagingLimit | DefaultPagingLimit
with static member Default = DefaultPagingLimit

type ObjectExpansion = StatusFullText | UserDetails | UserGeo | UserProfile | UserStats | UserWatch
with
    static member All = [StatusFullText; UserDetails; UserGeo; UserProfile; UserStats; UserWatch]
    static member None = Seq.empty<ObjectExpansion>

type ExtParams = ExtSubmission | ExtCamera | ExtStats
with
    static member All = [ExtSubmission; ExtCamera; ExtStats]
    static member None = Seq.empty<ExtParams>