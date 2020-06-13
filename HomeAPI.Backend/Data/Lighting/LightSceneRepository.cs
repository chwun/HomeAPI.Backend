using HomeAPI.Backend.Models.Lighting;
using Microsoft.EntityFrameworkCore;

namespace HomeAPI.Backend.Data.Lighting
{
	public class LightSceneRepository : AsyncRepository<LightScene>
	{
		public LightSceneRepository(DataContext context) : base(context)
		{
		}
	}
}