using System.Collections.Generic;
using SimpleFeedReader;

namespace HomeAPI.Backend.Models.News
{
	public interface ISimpleFeedAccess
	{
		IEnumerable<FeedItem> ReadFeed(string url);
	}
}