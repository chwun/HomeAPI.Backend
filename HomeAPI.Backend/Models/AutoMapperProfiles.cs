using AutoMapper;
using HomeAPI.Backend.Models.News;

namespace HomeAPI.Backend.Models
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
		{
			CreateMap<NewsFeedSubscriptionUpdateDTO, NewsFeedSubscription>();
		}
    }
}