using DeviantArtFs;
using DeviantArtFs.Extensions;
using DeviantArtFs.ParameterTypes;
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

            var allStashItems = await DeviantArtFs.Api.Stash.AsyncGetDelta(
                token,
                ExtParams.None,
                StashDeltaCursor.InitialStashDeltaRequest,
                PagingLimit.MaximumPagingLimit,
                PagingOffset.StartingOffset).ToListAsync();
            Console.WriteLine($"{allStashItems.Count} sta.sh items");
            Console.WriteLine();

            int i = 0;
            Stopwatch st = new();
            st.Start();
            await foreach (var deviation in DeviantArtFs.Api.Gallery.AsyncGetAllView(token, UserScope.ForCurrentUser, PagingLimit.MaximumPagingLimit, PagingOffset.StartingOffset)) {
                Console.WriteLine($"[{st.Elapsed}] {i + 1}. {deviation.title}");
                i++;
                if (i > 100) break;
            }
            st.Stop();
        }
    }
}
