namespace HomeAPI.Backend.Models.Lighting
{
	public interface ILightFactory
	{
		/// <summary>
		/// Creates a new common light object from this light object
		/// </summary>
		/// <param name="id">id of the new created light object</param>
		/// <returns>this light as common light object</returns>
		Light ToLight(int id);
	}
}