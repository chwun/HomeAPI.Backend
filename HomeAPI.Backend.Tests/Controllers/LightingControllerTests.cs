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
		[Fact]
		public async Task GetAllLights_ListNull()
		{
			IHueProvider hueProvider = Substitute.For<IHueProvider>();
			hueProvider.GetAllLightsAsync().Returns(Task.FromResult<List<Light>>(null));
			var controller = new LightingController(hueProvider);

			var result = await controller.GetAllLights();

			var actionResult = Assert.IsType<ActionResult<List<Light>>>(result);
			Assert.IsType<NoContentResult>(result.Result);
		}
	}
}