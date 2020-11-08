using System;
using System.Collections.Generic;

namespace HomeAPI.Backend.Models.News
{
	public class NewsFeedItemDTO
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string Summary { get; set; }
		public Uri Uri { get; set; }
		public IEnumerable<Uri> Images { get; set; }
		public DateTimeOffset PublishDate { get; set; }
		public DateTimeOffset LastUpdatedDate { get; set; }
	}
}