using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DeviantArtFs.Examples.WebApp.Models
{
    public class FeedItemModel
    {
        private readonly DeviantArtFeedItem _item;

        private Deviation Deviation => _item.GetDeviations().Where(x => !x.is_deleted).FirstOrDefault();
        private DeviantArtStatus Status => _item.GetStatus().Where(x => !x.is_deleted).FirstOrDefault();
        private DeviantArtFeedItemCollection Collection => _item.GetCollection().FirstOrDefault();

        public FeedItemModel(DeviantArtFeedItem item)
        {
            _item = item ?? throw new ArgumentNullException(nameof(item));
        }

        public string Type => _item.type;
        public string Username => _item.by_user.username;
        public string Usericon => _item.by_user.usericon;
        public string Url =>
            Deviation?.GetUrl()
            ?? Status?.GetUrl()
            ?? Collection?.url;
        public string ThumbnailUrl =>
            Deviation?.GetThumbs()?.FirstOrDefault()?.src;
        public string Title =>
            Deviation?.GetTitle();
        public string HTMLDescription =>
            _item.type == "collection_update" ? $"Updated collection <a href='{Collection.url}'>{WebUtility.HtmlEncode(Collection.name)}</a>"
            : _item.type == "status" ? Status?.GetBody()
            : Deviation?.GetExcerpt();
        public string TimeAgo {
            get {
                TimeSpan ts = DateTimeOffset.UtcNow - _item.ts.ToUniversalTime();
                if (ts.TotalMinutes < 1) return $"{(int)ts.TotalSeconds}s";
                if (ts.TotalHours < 1) return $"{(int)ts.TotalMinutes}m";
                if (ts.TotalDays < 1) return $"{(int)ts.TotalHours}h";
                if (ts.TotalDays < 30) return $"{(int)ts.TotalDays}d";
                return _item.ts.ToString();
            }
        }
    }
}
