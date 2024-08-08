using DeviantArtFs;
using DeviantArtFs.ParameterTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleConsoleApp2 {
    record Token(string AccessToken) : IDeviantArtAccessToken;

    class Program {
        static async Task Main() {
            Console.Write("Please enter a DeviantArt access token: ");
            var token = new Token(Console.ReadLine());

            int i = 0;
            Stopwatch st = new();
            st.Start();
            await foreach (var deviation in DeviantArtFs.Api.Gallery.GetAllViewAsync(token, UserScope.ForCurrentUser, PagingLimit.MaximumPagingLimit, PagingOffset.StartingOffset)) {
                Console.WriteLine($"[{st.Elapsed}] {i + 1}. {deviation.title}");
                i++;
                if (i > 100) break;
            }
            st.Stop();

            var user = await DeviantArtFs.Api.User.WhoamiAsync(token);
            i = 0;
            await foreach (var deviation in DeviantArtFs.Api.User.GetProfilePostsAsync(token, user.username, DeviantArtFs.Api.User.ProfilePostsCursor.FromBeginning)) {
                Console.WriteLine($"{i + 1}. {deviation.title}");
                i++;
                if (i > 100) break;
            }
        }
    }
}
