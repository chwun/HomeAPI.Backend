using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.Accounting;

namespace HomeAPI.Backend.Data.Accounting
{
    public interface IAccountingEntriesRepository
    {
        Task<List<AccountingEntry>> GetEntries(int categoryId);
        Task<AccountingEntry> GetEntry(int entryId);
        Task AddEntry(AccountingEntry entry);
        Task UpdateEntry(AccountingEntry entry);
        Task DeleteEntry(AccountingEntry entry);
    }
}