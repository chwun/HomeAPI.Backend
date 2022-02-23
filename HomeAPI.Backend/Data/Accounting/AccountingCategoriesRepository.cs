using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.Accounting;
using Microsoft.EntityFrameworkCore;

namespace HomeAPI.Backend.Data.Accounting
{
    public class AccountingCategoriesRepository : IAccountingCategoriesRepository
    {
        private readonly DataContext context;
        public AccountingCategoriesRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<List<AccountingCategory>> GetCategories()
        {
            return await context.AccountingCategories.AsNoTracking().ToListAsync();
        }

        public async Task<List<AccountingCategory>> GetCategoriesAsTree()
        {
            return await context.AccountingCategories
                .AsNoTracking()
                .Where(x => x.ParentCategory == null)
                .Include(x => x.SubCategories)
                .ToListAsync();
        }

        public async Task<AccountingCategory> GetCategory(int id)
        {
            return await context.AccountingCategories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AccountingCategory> GetCategoryWithSubCategories(int id)
        {
            return await context.AccountingCategories
                .Include(x => x.SubCategories)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        // public async Task<AccountingCategory> GetCategoryWithRelatedData(int id)
        // {
        // 	return await context.AccountingCategories
        // 		.AsNoTracking()
        // 		.Include(x => x.SubCategories)
        // 		.FirstOrDefaultAsync(x => x.Id == id);
        // }

        public async Task AddCategory(AccountingCategory category)
        {
            await context.AccountingCategories.AddAsync(category);
            await context.SaveChangesAsync();
        }

        public async Task UpdateCategory(AccountingCategory category)
        {
            context.AccountingCategories.Update(category);
            await context.SaveChangesAsync();
        }

        public async Task DeleteCategory(AccountingCategory category)
        {
            context.AccountingCategories.Remove(category);
            await context.SaveChangesAsync();
        }
    }
}