using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DeviantArtFs.Examples.WebApp.Models
{
    public class FeedItemModel
    {
        private readonly IBclDeviantArtFeedItem _item;

        public FeedItemModel(IBclDeviantArtFeedItem item)
        {
            _item = item ?? throw new ArgumentNullException(nameof(item));
        }

        private IBclDeviation Deviation => _item.Deviations.WhereNotDeleted().FirstOrDefault();
        private IBclDeviantArtStatus Status => new[] { _item.Status }.WhereNotDeleted().FirstOrDefault();
        private IBclDeviantArtFeedItemCollection Collection => _item.Collection;

        public string Type => _item.Type;
        public string Username => _item.ByUser.Username;
        public string Usericon => _item.ByUser.Usericon;
        public string Url =>
            Deviation?.Url
            ?? Status?.Url
            ?? Collection.Url;
        public string ThumbnailUrl =>
            Deviation?.Thumbs?.FirstOrDefault()?.Src;
        public string Title =>
            Deviation?.Title;
        public string HTMLDescription =>
            _item.Type == "collection_update" ? $"Updated collection <a href='{Collection?.Url}'>{WebUtility.HtmlEncode(Collection?.Name ?? "")}</a>"
            : _item.Type == "status" ? Status?.Body
            : Deviation?.Excerpt;
        public string TimeAgo {
            get {
                TimeSpan ts = DateTimeOffset.UtcNow - _item.Ts.ToUniversalTime();
                if (ts.TotalMinutes < 1) return $"{(int)ts.TotalSeconds}s";
                if (ts.TotalHours < 1) return $"{(int)ts.TotalMinutes}m";
                if (ts.TotalDays < 1) return $"{(int)ts.TotalHours}h";
                if (ts.TotalDays < 30) return $"{(int)ts.TotalDays}d";
                return _item.Ts.ToString();
            }
        }
    }
}
