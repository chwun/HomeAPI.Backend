using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Controllers;
using HomeAPI.Backend.Models.Lighting;
using HomeAPI.Backend.Providers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace HomeAPI.Backend.Tests.Controllers
{
	public class LightingControllerTests
	{
		#region GetAllLights

		[Fact]
		public async Task GetAllLights_ListNull()
		{
			var hueProvider = Substitute.For<IHueProvider>();
			List<Light> list = null;
			hueProvider.GetAllLightsAsync().Returns(Task.FromResult(list));
			var controller = new LightingController(hueProvider);

			var result = await controller.GetAllLights();

			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task GetAllLights_ListEmpty()
		{
			var hueProvider = Substitute.For<IHueProvider>();
			List<Light> list = new List<Light>();
			hueProvider.GetAllLightsAsync().Returns(Task.FromResult(list));
			var controller = new LightingController(hueProvider);

			var result = await controller.GetAllLights();

			Assert.IsType<NoContentResult>(result.Result);
		}

		[Fact]
		public async Task GetAllLights_ListValid()
		{
			var hueProvider = Substitute.For<IHueProvider>();
			List<Light> list = new List<Light>()
			{
				new Light()
				{
					Id = 5,
					Name = "Light No. 5",
					Type = LightType.HueDimmableLight,
					State = new LightState()
					{
						On = true,
						Brightness = 100,
						ColorTemperature = 10,
						Saturation = 50,
						Reachable = true,
					}
				}
			};
			hueProvider.GetAllLightsAsync().Returns(Task.FromResult(list));
			var controller = new LightingController(hueProvider);

			var result = await controller.GetAllLights();

			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var lights = Assert.IsType<List<Light>>(okResult.Value);
			Assert.Equal(list, lights);
		}

		#endregion GetAllLights

		#region GetLight

		[Fact]
		public async Task GetLight_NotFound()
		{
			var hueProvider = Substitute.For<IHueProvider>();
			Light light = null;
			hueProvider.GetLightByIdAsync(Arg.Any<int>()).Returns(Task.FromResult(light));
			var controller = new LightingController(hueProvider);

			var result = await controller.GetLight(5);

			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task GetLight_Found()
		{
			var hueProvider = Substitute.For<IHueProvider>();
			Light light = new Light()
			{
				Id = 5,
				Name = "Light No. 5",
				Type = LightType.HueDimmableLight,
				State = new LightState()
				{
					On = true,
					Brightness = 100,
					ColorTemperature = 10,
					Saturation = 50,
					Reachable = true,
				}
			};
			hueProvider.GetLightByIdAsync(Arg.Any<int>()).Returns(Task.FromResult(light));
			var controller = new LightingController(hueProvider);

			var result = await controller.GetLight(5);

			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var lightResult = Assert.IsType<Light>(okResult.Value);
			Assert.Equal(5, lightResult.Id);
		}

		#endregion GetLight

		#region SetLightState

		[Fact]
		public async Task SetLightState_Success()
		{
			var hueProvider = Substitute.For<IHueProvider>();
			var lightStateUpdate = new LightStateUpdate()
			{
				On = true,
				Brightness = 28,
				Saturation = 190,
				Hue = 8000,
				ColorTemperature = 103
			};
			bool success = true;
			hueProvider.SetLightStateAsync(Arg.Any<int>(), Arg.Any<LightStateUpdate>()).Returns(Task.FromResult(success));
			var controller = new LightingController(hueProvider);

			var result = await controller.SetLightState(5, lightStateUpdate);

			var okResult = result as OkResult;
			Assert.NotNull(okResult);
		}

		[Fact]
		public async Task SetLightState_NoSuccess()
		{
			var hueProvider = Substitute.For<IHueProvider>();
			var lightStateUpdate = new LightStateUpdate()
			{
				On = true,
				Brightness = 28,
				Saturation = 190,
				Hue = 8000,
				ColorTemperature = 103
			};
			bool success = false;
			hueProvider.SetLightStateAsync(Arg.Any<int>(), Arg.Any<LightStateUpdate>()).Returns(Task.FromResult(success));
			var controller = new LightingController(hueProvider);

			var result = await controller.SetLightState(5, lightStateUpdate);

			var notFoundResult = result as NotFoundResult;
			Assert.NotNull(notFoundResult);
		}

		#endregion SetLightState
	}
}