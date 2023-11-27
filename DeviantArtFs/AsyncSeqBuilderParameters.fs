namespace DeviantArtFs

open System.Threading.Tasks

type AsyncSeqBuilderParameters<'TPage, 'TItem, 'TOffset> = {
    initial_offset: 'TOffset
    get_page: 'TOffset -> Task<'TPage>
    extract_data: 'TPage -> seq<'TItem>
    has_more: 'TPage -> bool
    extract_next_offset: 'TPage -> 'TOffset
} with
    interface DeviantArtFs.AsyncSeq.AsyncSeqBuilder.IParameters<'TPage, 'TItem, 'TOffset> with
        member this.InitialOffset = this.initial_offset
        member this.GetPage(offset) = this.get_page offset
        member this.ExtractData(page) = this.extract_data page
        member this.HasMore(page) = this.has_more page
        member this.ExtractNextOffset(page) = this.extract_next_offset page