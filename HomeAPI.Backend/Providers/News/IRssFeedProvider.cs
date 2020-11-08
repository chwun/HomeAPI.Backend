using System.Collections.Generic;
using HomeAPI.Backend.Models.News;

namespace HomeAPI.Backend.Providers.News
{
    public interface IRssFeedProvider
    {
         List<NewsFeedItemDTO> ReadFeedContent(string url);
    }
}