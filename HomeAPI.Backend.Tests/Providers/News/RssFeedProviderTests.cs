using System.Collections.Generic;
using AutoMapper;
using HomeAPI.Backend.Models.News;
using HomeAPI.Backend.Providers.News;
using NSubstitute;
using SimpleFeedReader;
using Xunit;

namespace HomeAPI.Backend.Tests.Providers.News
{
	public class RssFeedProviderTests
	{
		#region ReadFeedContent

		[Fact]
		public void ReadFeedContent()
		{
			var feedItems = new List<FeedItem>()
			{
				new FeedItem()
				{
					Id = "id1",
					Summary = "summary1"
				},
				new FeedItem()
				{
					Id = "id2",
					Summary = "summary2"
				}
			};
			var newsFeedItems = new List<NewsFeedItemDTO>()
			{
				new NewsFeedItemDTO()
				{
					Id = "id1",
					Summary = "summary1"
				},
				new NewsFeedItemDTO()
				{
					Id = "id2",
					Summary = "summary2"
				}
			};

			ISimpleFeedAccess feedAccess = Substitute.For<ISimpleFeedAccess>();
			feedAccess.ReadFeed("example.com").Returns(feedItems);
			IMapper mapper = Substitute.For<IMapper>();
			mapper.Map<List<NewsFeedItemDTO>>(feedItems).Returns(newsFeedItems);

			var feedProvider = new RssFeedProvider(mapper, feedAccess);

			var result = feedProvider.ReadFeedContent("example.com");

			Assert.Equal(2, result.Count);
		}

		#endregion
	}
}