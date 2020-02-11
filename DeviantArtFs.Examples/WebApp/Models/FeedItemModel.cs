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

        private Deviation Deviation => _item.deviations.OrEmptyList().WhereNotDeleted().FirstOrDefault();
        private DeviantArtStatus Status => _item.status.OrNull() is DeviantArtStatus s && !s.is_deleted
            ? s
            : null;
        private DeviantArtFeedItemCollection Collection => _item.collection.OrNull();

        public string Type => _item.type;
        public string Username => _item.by_user.username;
        public string Usericon => _item.by_user.usericon;
        public string Url =>
            Deviation?.url?.OrNull()
            ?? Status?.url?.OrNull()
            ?? Collection?.url;
        public string ThumbnailUrl =>
            Deviation?.thumbs?.OrEmptyList()?.FirstOrDefault()?.src;
        public string Title =>
            Deviation?.title?.OrNull();
        public string HTMLDescription =>
            _item.type == "collection_update" ? $"Updated collection <a href='{Collection?.url}'>{WebUtility.HtmlEncode(Collection?.name ?? "")}</a>"
            : _item.type == "status" ? Status?.body?.OrNull()
            : Deviation?.excerpt?.OrNull();
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
