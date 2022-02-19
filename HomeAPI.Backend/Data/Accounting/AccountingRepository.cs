using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.Accounting;
using Microsoft.EntityFrameworkCore;

namespace HomeAPI.Backend.Data.Accounting
{
	public class AccountingRepository : IAccountingRepository
	{
		private readonly DataContext context;
		public AccountingRepository(DataContext context)
		{
			this.context = context;
		}

		public async Task<List<AccountingCategory>> GetAllCategories()
		{
			return await context.AccountingCategories.AsNoTracking().ToListAsync();
		}

		public async Task<AccountingCategory> GetCategory(int id)
		{
			return await context.AccountingCategories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
		}

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

		// public Task<List<AccountingItem>> GetAllItems()
		// {
		// 	throw new System.NotImplementedException();
		// }

		// public Task<List<AccountingSubCategory>> GetAllSubCategories()
		// {
		// 	throw new System.NotImplementedException();
		// }
	}
}