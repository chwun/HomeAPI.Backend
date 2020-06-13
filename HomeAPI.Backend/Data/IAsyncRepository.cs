using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeAPI.Backend.Data
{
	public interface IAsyncRepository<TModel> where TModel : class
	{
		Task<TModel> GetAsync(int id);

		Task<IEnumerable<TModel>> GetAllAsync();

		Task AddAsync(TModel entity);
		
		Task RemoveAsync(TModel entity);

		Task UpdateAsync(TModel entity);
	}
}