using System.Threading.Tasks;
using HomeAPI.Backend.Models.Lighting;
using Microsoft.EntityFrameworkCore;

namespace HomeAPI.Backend.Data.Lighting
{
	public class LightSceneRepository : AsyncRepository<LightScene>
	{
		public LightSceneRepository(DataContext context) : base(context)
		{
		}

		public async Task<LightScene> GetByNameAsync(string name)
		{
			return await DatabaseContext.Set<LightScene>().FirstOrDefaultAsync(x => x.Name.ToLower().Equals(name.ToLower()));
		}
	}
}