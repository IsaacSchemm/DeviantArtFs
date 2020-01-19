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

        private ExistingDeviation Deviation => _item.GetDeviations().SelectMany(x => x.SingleIfExists()).DefaultIfEmpty(null).First();
        private DeviantArtExistingStatus Status => _item.GetStatus().SelectMany(x => x.SingleIfExists()).DefaultIfEmpty(null).First();
        private DeviantArtFeedItemCollection Collection => _item.GetCollection().DefaultIfEmpty(null).First();

        public FeedItemModel(DeviantArtFeedItem item)
        {
            _item = item ?? throw new ArgumentNullException(nameof(item));
        }

        public string Type => _item.type;
        public string Username => _item.by_user.username;
        public string Usericon => _item.by_user.usericon;
        public string Url =>
            Deviation?.url
            ?? Status?.url
            ?? Collection?.url;
        public string ThumbnailUrl =>
            Deviation?.thumbs?.FirstOrDefault()?.src;
        public string Title =>
            Deviation?.title;
        public string HTMLDescription =>
            _item.type == "collection_update" ? $"Updated collection <a href='{Collection.url}'>{WebUtility.HtmlEncode(Collection.name)}</a>"
            : _item.type == "status" ? Status?.body
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
