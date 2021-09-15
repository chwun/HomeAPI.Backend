using HomeAPI.Backend.Models.Lighting;
using HomeAPI.Backend.Models.Lighting.Hue;
using Xunit;

namespace HomeAPI.Backend.Tests.Models.Lighting.Hue
{
	public class HueLightTests
	{
		[Theory]
		[InlineData("Dimmable Light", LightType.HueDimmableLight)]
		[InlineData("Extended Color Light", LightType.HueExtendedColorLight)]
		[InlineData("Color Temperature Light", LightType.HueColorTemperatureLight)]
		[InlineData("Unknown Light Type", LightType.None)]
		public void ToLight(string lightTypeName, LightType expectedLightType)
		{
			var hueLight = new HueLight()
			{
				Name = "Light No. 5",
				Type = lightTypeName,
				State = new HueLightState()
				{
					On = true,
					Bri = 100,
					Ct = 10,
					Sat = 50,
					Hue = 80,
					Reachable = true
				}
			};

			var result = hueLight.ToLight(5);

			Assert.Equal(5, result.Id);
			Assert.Equal("Light No. 5", result.Name);
			Assert.Equal(expectedLightType, result.Type);
			Assert.True(result.State.On);
			Assert.Equal(100, result.State.Brightness);
			Assert.Equal(10, result.State.ColorTemperature);
			Assert.Equal(50, result.State.Saturation);
			Assert.Equal(80, result.State.Hue);
			Assert.True(result.State.Reachable);
		}
	}
}