using HomeAPI.Backend.Models.Lighting;
using HomeAPI.Backend.Models.Lighting.Hue;
using Xunit;

namespace HomeAPI.Backend.Tests.Models.Lighting.Hue
{
	public class HueLightTests
	{
		[Fact]
		public void ToLight_Dimmable()
		{
			var hueLight = new HueLight()
			{
				Name = "Light No. 5",
				Type = "Dimmable light",
				State = new HueLightState()
				{
					On = true,
					Bri = 100,
					Ct = 10,
					Sat = 50,
					Reachable = true
				}
			};

			var result = hueLight.ToLight(5);

			Assert.Equal(5, result.Id);
			Assert.Equal("Light No. 5", result.Name);
			Assert.Equal(LightType.HueDimmableLight, result.Type);
			Assert.True(result.State.On);
			Assert.Equal(100, result.State.Brightness);
			Assert.Equal(10, result.State.ColorTemperature);
			Assert.Equal(50, result.State.Saturation);
			Assert.True(result.State.Reachable);
		}
	}
}