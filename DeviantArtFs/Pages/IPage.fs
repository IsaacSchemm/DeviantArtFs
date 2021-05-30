namespace DeviantArtFs.Pages

open DeviantArtFs.ParameterTypes

type IPage<'cursor, 'item> =
    abstract member NextPage: 'cursor option
    abstract member Items: 'item list

type ILinearPage<'item> =
    inherit IPage<PagingOffset, 'item>
    abstract member NextOffset: int option