using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeAPI.Backend.Models.Accounting;
using Microsoft.EntityFrameworkCore;

namespace HomeAPI.Backend.Data.Accounting
{
    public class AccountingEntriesRepository : IAccountingEntriesRepository
    {
        private readonly DataContext context;

        public AccountingEntriesRepository(DataContext context)
        {
            this.context = context;
        }

        public Task<List<AccountingEntry>> GetEntries(int categoryId)
        {
            return context.AccountingEntries
                .AsNoTracking()
                .Where(x => x.CategoryId == categoryId)
                .ToListAsync();
        }

        public Task<AccountingEntry> GetEntry(int entryId)
        {
            return context.AccountingEntries
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == entryId);
        }

        public async Task AddEntry(AccountingEntry entry)
        {
            await context.AccountingEntries.AddAsync(entry);
            await context.SaveChangesAsync();
        }

        public async Task UpdateEntry(AccountingEntry entry)
        {
            context.AccountingEntries.Update(entry);
            await context.SaveChangesAsync();
        }

        public async Task DeleteEntry(AccountingEntry entry)
        {
            context.AccountingEntries.Remove(entry);
            await context.SaveChangesAsync();
        }
    }
}