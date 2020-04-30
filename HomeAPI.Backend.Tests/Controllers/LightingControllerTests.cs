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
			IHueProvider hueProvider = Substitute.For<IHueProvider>();
			List<Light> list = null;
			hueProvider.GetAllLightsAsync().Returns(Task.FromResult(list));
			var controller = new LightingController(hueProvider);

			var result = await controller.GetAllLights();

			var actionResult = Assert.IsType<ActionResult<List<Light>>>(result);
			Assert.IsType<NoContentResult>(result.Result);
		}

		[Fact]
		public async Task GetAllLights_ListEmpty()
		{
			IHueProvider hueProvider = Substitute.For<IHueProvider>();
			List<Light> list = new List<Light>();
			hueProvider.GetAllLightsAsync().Returns(Task.FromResult(list));
			var controller = new LightingController(hueProvider);

			var result = await controller.GetAllLights();

			var actionResult = Assert.IsType<ActionResult<List<Light>>>(result);
			Assert.IsType<NoContentResult>(result.Result);
		}

		[Fact]
		public async Task GetAllLights_ListValid()
		{
			IHueProvider hueProvider = Substitute.For<IHueProvider>();
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
						Reachable = true
					}
				}
			};
			hueProvider.GetAllLightsAsync().Returns(Task.FromResult(list));
			var controller = new LightingController(hueProvider);

			var result = await controller.GetAllLights();

			var actionResult = Assert.IsType<ActionResult<List<Light>>>(result);
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var lights = Assert.IsType<List<Light>>(okResult.Value);
			Assert.Equal(list, lights);
		}

		#endregion GetAllLights
	}
}