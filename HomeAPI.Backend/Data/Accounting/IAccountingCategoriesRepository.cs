using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.Accounting;

namespace HomeAPI.Backend.Data.Accounting
{
	public interface IAccountingCategoriesRepository
	{
		Task<List<AccountingCategory>> GetAllCategories();
		Task<AccountingCategory> GetCategory(int id);
		Task<AccountingCategory> GetCategoryWithRelatedData(int id);
		Task AddCategory(AccountingCategory category);
		Task UpdateCategory(AccountingCategory category);
		Task DeleteCategory(AccountingCategory category);
	}
}