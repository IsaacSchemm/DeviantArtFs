using DeviantArtFs;
using DeviantArtFs.Extensions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleConsoleApp2 {
    record Token : IDeviantArtAccessToken {
        public string AccessToken { get; init; }
    }

    class Program {
        static async Task Main() {
            Console.Write("Please enter a DeviantArt access token: ");
            var token = new Token { AccessToken = Console.ReadLine() };

            var req = new DeviantArtFs.Api.Gallery.GalleryAllViewRequest();
            int i = 0;
            Stopwatch st = new Stopwatch();
            st.Start();
            await foreach (var deviation in DeviantArtFs.Api.Gallery.GalleryAllView.ToAsyncSeq(token, req, 0).ToAsyncEnumerable()) {
                Console.WriteLine($"[{st.Elapsed}] {i + 1}. {deviation.title}");
                i++;
                if (i > 100) break;
            }
            st.Stop();
        }
    }
}
