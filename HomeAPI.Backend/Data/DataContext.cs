using HomeAPI.Backend.Models.Lighting;
using HomeAPI.Backend.Models.News;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HomeAPI.Backend.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options)
		: base(options)
		{
		}

		public DbSet<LightScene> LightScenes { get; set; }

		public DbSet<NewsFeedSubscription> NewsFeedSubscriptions { get; set; }
	}
}