using System.ComponentModel.DataAnnotations;

namespace HomeAPI.Backend.Models.News
{
	public class NewsFeedSubscriptionUpdateDTO
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string Url { get; set; }
	}
}