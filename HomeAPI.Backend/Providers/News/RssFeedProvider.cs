using System.Collections.Generic;
using AutoMapper;
using HomeAPI.Backend.Models.News;

namespace HomeAPI.Backend.Providers.News
{
	public class RssFeedProvider : IRssFeedProvider
	{
		private readonly ISimpleFeedAccess feedAccess;
		private readonly IMapper mapper;

		public RssFeedProvider(IMapper mapper, ISimpleFeedAccess feedAccess)
		{
			this.feedAccess = feedAccess;
			this.mapper = mapper;
		}

		public List<NewsFeedItemDTO> ReadFeedContent(string url)
		{
			var items = feedAccess.ReadFeed(url);

			return mapper.Map<List<NewsFeedItemDTO>>(items);
		}
	}
}