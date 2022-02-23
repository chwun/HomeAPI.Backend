using System;
using HomeAPI.Backend.Models.Accounting;
using HomeAPI.Backend.Models.Lighting;
using HomeAPI.Backend.Models.News;
using Microsoft.EntityFrameworkCore;

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

        public DbSet<AccountingCategory> AccountingCategories { get; set; }
        public DbSet<AccountingEntry> AccountingEntries { get; set; }
    }
}