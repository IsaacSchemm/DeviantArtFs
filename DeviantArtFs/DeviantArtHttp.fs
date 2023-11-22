namespace DeviantArtFs

open System.Net.Http
open System.Collections.Generic
open System.IO

module DeviantArtHttp =
    let mutable HttpClient =
        let c = new HttpClient()
        c.DefaultRequestHeaders.Add("User-Agent", "DeviantArtFs/9.0 (https://github.com/IsaacSchemm/DeviantArtFs)")
        c

    let internal createForm items =
        new FormUrlEncodedContent([
            for (k, v) in items do
                new KeyValuePair<string, string>(k, v)
        ])

    let internal createQueryString items =
        use content = createForm items
        use s = content.ReadAsStreamAsync().GetAwaiter().GetResult()
        use sr = new StreamReader(s)
        sr.ReadToEnd()