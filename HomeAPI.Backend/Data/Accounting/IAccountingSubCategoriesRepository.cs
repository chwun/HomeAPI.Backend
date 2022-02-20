using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.Accounting;

namespace HomeAPI.Backend.Data.Accounting
{
	public interface IAccountingSubCategoriesRepository
	{
		Task<List<AccountingSubCategory>> GetAllSubCategories();
		Task<AccountingSubCategory> GetSubCategory(int id);
		Task<AccountingSubCategory> GetSubCategoryWithRelatedData(int id);
		Task UpdateSubCategory(AccountingSubCategory subCategory);
		Task DeleteSubCategory(AccountingSubCategory subCategory);
	}
}