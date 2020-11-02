using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using HomeAPI.Backend.Models.News;

namespace HomeAPI.Backend.Models
{
	[ExcludeFromCodeCoverage]
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
		{
			CreateMap<NewsFeedSubscriptionUpdateDTO, NewsFeedSubscription>();
		}
    }
}