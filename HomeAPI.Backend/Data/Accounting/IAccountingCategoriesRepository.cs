using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.Accounting;

namespace HomeAPI.Backend.Data.Accounting
{
    public interface IAccountingCategoriesRepository
    {
        Task<List<AccountingCategory>> GetCategories();
        Task<List<AccountingCategory>> GetCategoriesAsTree();
        Task<AccountingCategory> GetCategory(int id);
        Task<AccountingCategory> GetCategoryWithSubCategories(int id);
        Task AddCategory(AccountingCategory category);
        Task UpdateCategory(AccountingCategory category);
        Task DeleteCategory(AccountingCategory category);
    }
}