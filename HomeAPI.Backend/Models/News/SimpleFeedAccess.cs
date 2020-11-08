using System.Collections.Generic;
using SimpleFeedReader;

namespace HomeAPI.Backend.Models.News
{
	public class SimpleFeedAccess : ISimpleFeedAccess
	{
		private readonly FeedReader feedReader;

		public SimpleFeedAccess()
		{
			feedReader = new FeedReader();
		}

		public IEnumerable<FeedItem> ReadFeed(string url)
		{
			return feedReader.RetrieveFeed(url);
		}
	}
}