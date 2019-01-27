using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviantArtFs.Examples.WebApp.Models
{
    public class FeedModel
    {
        private IBclDeviantArtFeedCursorResult _result;

        public FeedModel(IBclDeviantArtFeedCursorResult result)
        {
            _result = result ?? throw new ArgumentNullException(nameof(result));
        }

        public string Cursor => _result.Cursor;
        public bool HasMore => _result.HasMore;
        public IEnumerable<FeedItemModel> Items => _result.Items.Select(x => new FeedItemModel(x));
    }
}
