using HomeAPI.Backend.Models.Lighting;
using HomeAPI.Backend.Models.Lighting.Hue;
using Xunit;

namespace HomeAPI.Backend.Tests.Models.Lighting.Hue
{
	public class HueLightStateUpdateFactoryTests
	{
		[Fact]
		public void CreateFromLightState_StateNull()
		{
			IHueLightStateUpdateFactory lightStateUpdateFactory = new HueLightStateUpdateFactory();
			LightStateUpdate lightStateUpdate = null;

			var result = lightStateUpdateFactory.CreateFromLightState(LightType.HueDimmableLight, lightStateUpdate);

			Assert.Null(result);
		}

		[Fact]
		public void CreateFromLightState_Dimmable()
		{
			IHueLightStateUpdateFactory lightStateUpdateFactory = new HueLightStateUpdateFactory();

			var lightStateUpdate = new LightStateUpdate()
			{
				On = true,
				Brightness = 90,
				ColorTemperature = 55,
				Saturation = 27,
				Hue = 8000
			};

			var result = lightStateUpdateFactory.CreateFromLightState(LightType.HueDimmableLight, lightStateUpdate) as HueLightStateUpdateDimmable;

			Assert.True(result.On);
			Assert.Equal(90, result.Brightness);
		}

				[Fact]
		public void CreateFromLightState_ExtendedColor()
		{
			IHueLightStateUpdateFactory lightStateUpdateFactory = new HueLightStateUpdateFactory();

			var lightStateUpdate = new LightStateUpdate()
			{
				On = true,
				Brightness = 90,
				ColorTemperature = 55,
				Saturation = 27,
				Hue = 8000
			};

			var result = lightStateUpdateFactory.CreateFromLightState(LightType.HueExtendedColorLight, lightStateUpdate) as HueLightStateUpdateExtendedColor;

			Assert.True(result.On);
			Assert.Equal(90, result.Brightness);
			Assert.Equal(55, result.ColorTemperature);
			Assert.Equal(27, result.Saturation);
			Assert.Equal(8000, result.Hue);
		}
	}
}