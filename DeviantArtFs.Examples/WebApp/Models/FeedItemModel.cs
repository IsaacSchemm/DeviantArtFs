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

        public FeedItemModel(DeviantArtFeedItem item)
        {
            _item = item ?? throw new ArgumentNullException(nameof(item));
        }

        public string Type => _item.type;
        public string Username => _item.by_user.username;
        public string Usericon => _item.by_user.usericon;
        public string Url =>
            _item.GetDeviations().SelectMany(x => x.SingleIfExists()).Select(x => x.url).FirstOrDefault()
            ?? _item.GetStatus().SelectMany(x => x.SingleIfExists()).Select(x => x.url).FirstOrDefault()
            ?? _item.GetCollection().Select(x => x.url).FirstOrDefault();
        public string ThumbnailUrl =>
            _item.GetDeviations().SelectMany(x => x.SingleIfExists()).FirstOrDefault()?.thumbs?.FirstOrDefault()?.src;
        public string Title =>
            _item.GetDeviations().SelectMany(x => x.SingleIfExists()).FirstOrDefault()?.title;
        public string HTMLDescription =>
            _item.type == "collection_update" ? $"Updated collection <a href='{_item.GetCollection().First().url}'>{WebUtility.HtmlEncode(_item.GetCollection().First().name)}</a>"
            : _item.type == "status" ? _item.GetStatus().SelectMany(x => x.SingleIfExists()).First()?.body
            : _item.GetDeviations().SelectMany(x => x.SingleIfExists()).FirstOrDefault()?.GetExcerpt();
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
