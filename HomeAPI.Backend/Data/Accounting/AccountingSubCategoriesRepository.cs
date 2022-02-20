using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.Accounting;
using Microsoft.EntityFrameworkCore;

namespace HomeAPI.Backend.Data.Accounting
{
	public class AccountingSubCategoriesRepository : IAccountingSubCategoriesRepository
	{
		private readonly DataContext context;
		public AccountingSubCategoriesRepository(DataContext context)
		{
			this.context = context;
		}

		public async Task<List<AccountingSubCategory>> GetAllSubCategories()
		{
			return await context.AccountingSubCategories.AsNoTracking().ToListAsync();
		}

		public async Task<AccountingSubCategory> GetSubCategory(int id)
		{
			return await context.AccountingSubCategories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<AccountingSubCategory> GetSubCategoryWithRelatedData(int id)
		{
			return await context.AccountingSubCategories
				.AsNoTracking()
				.Include(x => x.Category)
				.Include(x => x.Items)
				.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task UpdateSubCategory(AccountingSubCategory subCategory)
		{
			context.AccountingSubCategories.Update(subCategory);
			await context.SaveChangesAsync();
		}

		public async Task DeleteSubCategory(AccountingSubCategory subCategory)
		{
			context.AccountingSubCategories.Remove(subCategory);
			await context.SaveChangesAsync();
		}
	}
}