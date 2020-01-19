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
            _item.GetDeviations().Select(x => x.url).FirstOrDefault()
            ?? _item.GetStatuses().SelectMany(x => x.ToExistingStatuses()).Select(x => x.url).FirstOrDefault()
            ?? _item.GetCollections().Select(x => x.url).FirstOrDefault();
        public string ThumbnailUrl =>
            _item.GetDeviations().FirstOrDefault()?.thumbs?.FirstOrDefault()?.Src;
        public string Title =>
            _item.GetDeviations().FirstOrDefault()?.title;
        public string HTMLDescription =>
            _item.type == "collection_update" ? $"Updated collection <a href='{_item.GetCollections().First().url}'>{WebUtility.HtmlEncode(_item.GetCollections().First().name)}</a>"
            : _item.type == "status" ? _item.GetStatuses().First()?.body
            : _item.GetDeviations()?.FirstOrDefault()?.excerpt;
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
