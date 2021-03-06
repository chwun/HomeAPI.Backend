using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using HomeAPI.Backend.Models.News;
using SimpleFeedReader;

namespace HomeAPI.Backend.Models
{
	[ExcludeFromCodeCoverage]
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles()
		{
			CreateMap<NewsFeedSubscriptionUpdateDTO, NewsFeedSubscription>();
			CreateMap<FeedItem, NewsFeedItemDTO>();
		}
	}
}