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

        public string Url =>
            _item.Deviations.FirstOrDefault()?.Url
            ?? _item.Status?.Url
            ?? _item.Collection.Url;
        public string ThumbnailUrl =>
            _item.Deviations.FirstOrDefault()?.Thumbs?.FirstOrDefault()?.Src;
        public string Title =>
            _item.Deviations.FirstOrDefault()?.Title;
        public string Username => _item.ByUser.Username;
        public string Usericon => _item.ByUser.Usericon;
        public string HTMLDescription =>
            _item.Type == "collection_update" ? $"Updated collection <a href='{_item.Collection.Url}'>{WebUtility.HtmlEncode(_item.Collection.Name)}</a>"
            : _item.Type == "status" ? _item.Status?.Body
            : _item.Deviations?.FirstOrDefault()?.Excerpt;
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
