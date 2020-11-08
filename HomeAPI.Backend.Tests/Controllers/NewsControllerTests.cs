using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HomeAPI.Backend.Controllers;
using HomeAPI.Backend.Data;
using HomeAPI.Backend.Models.News;
using HomeAPI.Backend.Providers.News;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace HomeAPI.Backend.Tests.Controllers
{
	public class NewsControllerTests
	{
		private readonly IMapper mapper;
		private readonly IRssFeedProvider rssFeedProvider;
		private readonly IAsyncRepository<NewsFeedSubscription> subscriptionRepository;

		private readonly List<NewsFeedSubscription> newsFeedSubscriptions;

		public NewsControllerTests()
		{
			mapper = Substitute.For<IMapper>();
			rssFeedProvider = Substitute.For<IRssFeedProvider>();
			subscriptionRepository = Substitute.For<IAsyncRepository<NewsFeedSubscription>>();

			newsFeedSubscriptions = new List<NewsFeedSubscription>()
			{
				new NewsFeedSubscription()
				{
					Id = 1,
					Name = "test feed 1",
					Url = "example.com"
				},
				new NewsFeedSubscription()
				{
					Id = 2,
					Name = "test feed 2",
					Url = "example2.com"
				}
			};
		}

		#region GetNewsFeedSubscriptions

		[Fact]
		public async Task GetNewsFeedSubscriptions_NotFound()
		{
			subscriptionRepository.GetAllAsync().Returns((List<NewsFeedSubscription>)null);
			NewsController controller = new NewsController(mapper, rssFeedProvider, subscriptionRepository);

			var result = await controller.GetNewsFeedSubscriptions();

			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task GetNewsFeedSubscriptions_Success()
		{
			subscriptionRepository.GetAllAsync().Returns(newsFeedSubscriptions);
			NewsController controller = new NewsController(mapper, rssFeedProvider, subscriptionRepository);

			var result = await controller.GetNewsFeedSubscriptions();

			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var resultList = Assert.IsType<List<NewsFeedSubscription>>(okResult.Value);
			Assert.Equal(newsFeedSubscriptions, resultList);
		}

		#endregion

		#region GetNewsFeedSubscription

		[Fact]
		public async Task GetNewsFeedSubscription_NotFound()
		{
			subscriptionRepository.GetAsync(Arg.Any<int>()).Returns((NewsFeedSubscription)null);
			NewsController controller = new NewsController(mapper, rssFeedProvider, subscriptionRepository);

			var result = await controller.GetNewsFeedSubscription(1);

			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task GetNewsFeedSubscription_Success()
		{
			subscriptionRepository.GetAsync(1).Returns(newsFeedSubscriptions[0]);
			NewsController controller = new NewsController(mapper, rssFeedProvider, subscriptionRepository);

			var result = await controller.GetNewsFeedSubscription(1);

			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var resultObject = Assert.IsType<NewsFeedSubscription>(okResult.Value);
			Assert.Equal(newsFeedSubscriptions[0], resultObject);
		}

		#endregion

		#region AddNewsFeedSubscription

		[Fact]
		public async Task AddNewsFeedSubscription_BadRequest()
		{
			NewsController controller = new NewsController(mapper, rssFeedProvider, subscriptionRepository);

			var result = await controller.AddNewsFeedSubscription(null);

			Assert.IsType<BadRequestResult>(result.Result);
		}

		[Fact]
		public async Task AddNewsFeedSubscription_Created()
		{
			NewsController controller = new NewsController(mapper, rssFeedProvider, subscriptionRepository);
			NewsFeedSubscriptionUpdateDTO feedSubscriptionDTO = new NewsFeedSubscriptionUpdateDTO()
			{
				Name = "new name",
				Url = "newUrl.com"
			};
			NewsFeedSubscription feedSubscription = new NewsFeedSubscription()
			{
				Id = 0,
				Name = "new name",
				Url = "newUrl.com"
			};
			mapper.Map<NewsFeedSubscription>(feedSubscriptionDTO).Returns(feedSubscription);
			subscriptionRepository.When(x => x.AddAsync(feedSubscription))
									.Do(x => feedSubscription.Id = 5);

			var result = await controller.AddNewsFeedSubscription(feedSubscriptionDTO);

			var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
			var resultObject = Assert.IsType<NewsFeedSubscription>(createdResult.Value);
			Assert.Equal(5, resultObject.Id);
		}

		#endregion

		#region UpdateNewsFeedSubscription

		[Fact]
		public async Task UpdateNewsFeedSubscription_BadRequest()
		{
			NewsController controller = new NewsController(mapper, rssFeedProvider, subscriptionRepository);

			var result = await controller.UpdateNewsFeedSubscription(1, null);

			Assert.IsType<BadRequestResult>(result.Result);
		}

		[Fact]
		public async Task UpdateNewsFeedSubscription_Ok()
		{
			NewsController controller = new NewsController(mapper, rssFeedProvider, subscriptionRepository);
			NewsFeedSubscriptionUpdateDTO feedSubscriptionDTO = new NewsFeedSubscriptionUpdateDTO()
			{
				Name = "new name",
				Url = "newUrl.com"
			};
			NewsFeedSubscription feedSubscription = new NewsFeedSubscription()
			{
				Id = 0,
				Name = "new name",
				Url = "newUrl.com"
			};
			mapper.Map<NewsFeedSubscription>(feedSubscriptionDTO).Returns(feedSubscription);

			var result = await controller.UpdateNewsFeedSubscription(1, feedSubscriptionDTO);

			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var resultObject = Assert.IsType<NewsFeedSubscription>(okResult.Value);
			Assert.Equal(1, resultObject.Id);
			Assert.Equal("new name", resultObject.Name);
			Assert.Equal("newUrl.com", resultObject.Url);
		}

		#endregion

		#region DeleteNewsFeedSubscription

		[Fact]
		public async Task DeleteNewsFeedSubscription_NotFound()
		{
			subscriptionRepository.GetAsync(Arg.Any<int>()).Returns((NewsFeedSubscription)null);
			NewsController controller = new NewsController(mapper, rssFeedProvider, subscriptionRepository);

			var result = await controller.DeleteNewsFeedSubscription(1);

			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task DeleteNewsFeedSubscription_Success()
		{
			subscriptionRepository.GetAsync(1).Returns(newsFeedSubscriptions[0]);
			NewsController controller = new NewsController(mapper, rssFeedProvider, subscriptionRepository);

			var result = await controller.DeleteNewsFeedSubscription(1);

			Assert.IsType<NoContentResult>(result);
		}

		#endregion

		#region GetNewsFeedSubscriptionContent

		[Fact]
		public async Task GetNewsFeedSubscriptionContent_NotFound()
		{
			subscriptionRepository.GetAsync(Arg.Any<int>()).Returns((NewsFeedSubscription)null);
			NewsController controller = new NewsController(mapper, rssFeedProvider, subscriptionRepository);

			var result = await controller.GetNewsFeedSubscriptionContent(1);

			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task GetNewsFeedSubscriptionContent_Success()
		{
			subscriptionRepository.GetAsync(1).Returns(newsFeedSubscriptions[0]);
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
			rssFeedProvider.ReadFeedContent("example.com").Returns(newsFeedItems);
			NewsController controller = new NewsController(mapper, rssFeedProvider, subscriptionRepository);

			var result = await controller.GetNewsFeedSubscriptionContent(1);

			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var resultObject = Assert.IsType<List<NewsFeedItemDTO>>(okResult.Value);
			Assert.Equal(2, resultObject.Count);
			Assert.Equal("id1", resultObject[0].Id);
			Assert.Equal("summary1", resultObject[0].Summary);
			Assert.Equal("id2", resultObject[1].Id);
			Assert.Equal("summary2", resultObject[1].Summary);
		}

		#endregion
	}
}