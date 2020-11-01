using HomeAPI.Backend.Models.News;

namespace HomeAPI.Backend.Data.News
{
	public class NewsFeedSubscriptionRepository : AsyncRepository<NewsFeedSubscription>
	{
		public NewsFeedSubscriptionRepository(DataContext context) : base(context)
		{

		}
	}
}