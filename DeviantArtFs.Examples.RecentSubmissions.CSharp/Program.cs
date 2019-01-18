using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviantArtFs.Examples.RecentSubmissions.CSharp
{
    class Token : IDeviantArtAccessToken
    {
        public string AccessToken { get; private set; }

        public Token(string accessToken)
        {
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        }
    }

    class Program
    {
        static string GetToken()
        {
            string token_file = "token.txt";
            string token_string = File.Exists(token_file)
                ? File.ReadAllText(token_file)
                : "";

            bool valid = Requests.Util.Placebo.IsValidAsync(new Token(token_string)).GetAwaiter().GetResult();
            if (valid)
            {
                return token_string;
            }
            else
            {
                Console.Write("Please enter the client ID (e.g. 1234): ");
                int client_id = int.Parse(Console.ReadLine());

                Console.Write("Please enter the redirect URL (default: https://www.example.com): ");
                string url1 = Console.ReadLine();
                string url2 = url1 == ""
                    ? "https://www.example.com"
                    : url1;

                using (var form = new WinForms.DeviantArtImplicitGrantForm(client_id, new Uri(url2), new[] { "browse", "user", "stash", "publish", "user.manage" }))
                {
                    if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    {
                        throw new Exception("Login cancelled");
                    }
                    else
                    {
                        File.WriteAllText(token_file, form.AccessToken);
                        return form.AccessToken;
                    }
                }
            }
        }

        static PagingParams Page(int offset, int limit) => new PagingParams { Offset = offset, Limit = limit };

        static async Task Sandbox(string token_string)
        {
            var token = new Token(token_string);

            Console.Write("Enter a username (leave blank to see your own submissions): ");
            string read = Console.ReadLine();
            Console.WriteLine();

            var me = await Requests.User.Whoami.ExecuteAsync(token);

            string username = read == ""
                ? me.Username
                : read;

            var deviations = await Requests.Gallery.GalleryAllView.ExecuteAsync(
                token,
                Page(0, 1),
                new Requests.Gallery.GalleryAllViewRequest { Username = username });
            var deviation = deviations.Results.FirstOrDefault();
            if (deviation != null)
            {
                Console.WriteLine($"Most recent deviation: {deviation.Title} ({deviation.PublishedTime})");

                var metadata = await Requests.Deviation.MetadataById.ExecuteAsync(
                    token,
                    new Requests.Deviation.MetadataRequest(new[] { deviation.Deviationid }));
                foreach (var m in metadata)
                {
                    Console.WriteLine(string.Join(" ", m.Tags.Select(t => $"#{t.TagName}")));
                }

                var favorites = await Requests.Deviation.WhoFaved.ToArrayAsync(
                    token,
                    deviation.Deviationid,
                    0,
                    int.MaxValue);
                if (favorites.Any())
                {
                    Console.WriteLine("Favorited by:");
                    foreach (var f in favorites)
                    {
                        Console.WriteLine($"    {f.User.Username} {f.Time}");
                    }
                }

                var comments_req = new Requests.Comments.DeviationCommentsRequest(deviation.Deviationid) { Maxdepth = 5 };
                var comments = await Requests.Comments.DeviationComments.ToArrayAsync(
                    token,
                    comments_req,
                    0,
                    int.MaxValue);
                if (comments.Any())
                {
                    Console.WriteLine("Comments by:");
                    foreach (var c in comments)
                    {
                        Console.WriteLine($"    {c.User.Username} {c.Body}");
                    }
                }

                Console.WriteLine();
            }

            var journals = await Requests.Browse.UserJournals.ExecuteAsync(
                token,
                Page(0, 1),
                new Requests.Browse.UserJournalsRequest(username) { Featured = false });
            var journal = journals.Results.FirstOrDefault();
            if (journal != null)
            {
                Console.WriteLine($"Most recent journal: {journal.Title} ({journal.PublishedTime})");

                var metadata = await Requests.Deviation.MetadataById.ExecuteAsync(
                    token,
                    new Requests.Deviation.MetadataRequest(new[] { journal.Deviationid }));
                foreach (var m in metadata)
                {
                    Console.WriteLine(string.Join(" ", m.Tags.Select(t => $"#{t.TagName}")));
                }

                var favorites = await Requests.Deviation.WhoFaved.ToArrayAsync(
                    token,
                    journal.Deviationid,
                    0,
                    int.MaxValue);
                if (favorites.Any())
                {
                    Console.WriteLine("Favorited by:");
                    foreach (var f in favorites)
                    {
                        Console.WriteLine($"    {f.User.Username} {f.Time}");
                    }
                }

                var comments_req = new Requests.Comments.DeviationCommentsRequest(journal.Deviationid) { Maxdepth = 5 };
                var comments = await Requests.Comments.DeviationComments.ToArrayAsync(
                    token,
                    comments_req,
                    0,
                    int.MaxValue);
                if (comments.Any())
                {
                    Console.WriteLine("Comments by:");
                    foreach (var c in comments)
                    {
                        Console.WriteLine($"    {c.User.Username} {c.Body}");
                    }
                }

                Console.WriteLine();
            }

            var statuses = await Requests.User.StatusesList.ExecuteAsync(
                token,
                Page(0, 1),
                username);
            var status = statuses.Results.FirstOrDefault();
            if (status != null)
            {
                Console.WriteLine($"Most recent status: {status.Body} ({status.Ts})");

                var comments_req = new Requests.Comments.StatusCommentsRequest(status.Statusid) { Maxdepth = 5 };
                var comments = await Requests.Comments.StatusComments.ToArrayAsync(
                    token,
                    comments_req,
                    0,
                    int.MaxValue);
                if (comments.Any())
                {
                    Console.WriteLine("Comments:");
                }
                foreach (var c in comments)
                {
                    Console.WriteLine($"    {c.User.Username}: {c.Body}");
                }

                Console.WriteLine();
            }
        }

        [STAThread]
        static int Main(string[] args)
        {
            string token_string = GetToken();
            Sandbox(token_string).GetAwaiter().GetResult();
            return 0;
        }
    }
}
