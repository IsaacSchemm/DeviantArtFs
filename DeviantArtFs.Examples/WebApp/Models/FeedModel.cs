using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviantArtFs.Examples.WebApp.Models
{
    public class FeedModel
    {
        private DeviantArtFeedCursorResult _result;

        public FeedModel(DeviantArtFeedCursorResult result)
        {
            _result = result ?? throw new ArgumentNullException(nameof(result));
        }

        public string Cursor => _result.cursor;
        public bool HasMore => _result.has_more;
        public IEnumerable<FeedItemModel> Items => _result.items.Select(x => new FeedItemModel(x));
    }
}
