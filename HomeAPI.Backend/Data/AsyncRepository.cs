using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HomeAPI.Backend.Data
{
	public abstract class AsyncRepository<TModel> : IAsyncRepository<TModel> where TModel : class
	{
		protected readonly DbContext DatabaseContext;

		public AsyncRepository(DbContext context)
		{
			this.DatabaseContext = context;
		}

		public async Task<TModel> GetAsync(int id)
		{
			return await DatabaseContext.Set<TModel>().FindAsync(id);
		}

		public async Task<IEnumerable<TModel>> GetAllAsync()
		{
			return await DatabaseContext.Set<TModel>().ToListAsync();
		}

		public async Task AddAsync(TModel entity)
		{
			await DatabaseContext.Set<TModel>().AddAsync(entity);
			await DatabaseContext.SaveChangesAsync();
		}

		public async Task RemoveAsync(TModel entity)
		{
			DatabaseContext.Set<TModel>().Remove(entity);
			await DatabaseContext.SaveChangesAsync();
		}

		public async Task UpdateAsync(TModel entity)
		{
			DatabaseContext.Set<TModel>().Update(entity);
			await DatabaseContext.SaveChangesAsync();
		}
	}
}