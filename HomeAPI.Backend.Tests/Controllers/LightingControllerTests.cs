using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Controllers;
using HomeAPI.Backend.Data;
using HomeAPI.Backend.Models.Lighting;
using HomeAPI.Backend.Providers.Lighting;
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
			var hueLightProvider = Substitute.For<IHueLightProvider>();
			List<Light> list = null;
			hueLightProvider.GetAllLightsAsync().Returns(Task.FromResult(list));
			var controller = new LightingController(hueLightProvider, null);

			var result = await controller.GetAllLights();

			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task GetAllLights_ListEmpty()
		{
			var hueLightProvider = Substitute.For<IHueLightProvider>();
			List<Light> list = new List<Light>();
			hueLightProvider.GetAllLightsAsync().Returns(Task.FromResult(list));
			var controller = new LightingController(hueLightProvider, null);

			var result = await controller.GetAllLights();

			Assert.IsType<NoContentResult>(result.Result);
		}

		[Fact]
		public async Task GetAllLights_ListValid()
		{
			var hueLightProvider = Substitute.For<IHueLightProvider>();
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
			hueLightProvider.GetAllLightsAsync().Returns(Task.FromResult(list));
			var controller = new LightingController(hueLightProvider, null);

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
			var hueLightProvider = Substitute.For<IHueLightProvider>();
			Light light = null;
			hueLightProvider.GetLightByIdAsync(Arg.Any<int>()).Returns(Task.FromResult(light));
			var controller = new LightingController(hueLightProvider, null);

			var result = await controller.GetLight(5);

			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task GetLight_Found()
		{
			var hueLightProvider = Substitute.For<IHueLightProvider>();
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
			hueLightProvider.GetLightByIdAsync(Arg.Any<int>()).Returns(Task.FromResult(light));
			var controller = new LightingController(hueLightProvider, null);

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
			var hueLightProvider = Substitute.For<IHueLightProvider>();
			var lightStateUpdate = new LightStateUpdate()
			{
				On = true,
				Brightness = 28,
				Saturation = 190,
				Hue = 8000,
				ColorTemperature = 103
			};
			bool success = true;
			hueLightProvider.SetLightStateAsync(Arg.Any<int>(), Arg.Any<LightStateUpdate>()).Returns(Task.FromResult(success));
			var controller = new LightingController(hueLightProvider, null);

			var result = await controller.SetLightState(5, lightStateUpdate);

			var okResult = result as OkResult;
			Assert.NotNull(okResult);
		}

		[Fact]
		public async Task SetLightState_NoSuccess()
		{
			var hueLightProvider = Substitute.For<IHueLightProvider>();
			var lightStateUpdate = new LightStateUpdate()
			{
				On = true,
				Brightness = 28,
				Saturation = 190,
				Hue = 8000,
				ColorTemperature = 103
			};
			bool success = false;
			hueLightProvider.SetLightStateAsync(Arg.Any<int>(), Arg.Any<LightStateUpdate>()).Returns(Task.FromResult(success));
			var controller = new LightingController(hueLightProvider, null);

			var result = await controller.SetLightState(5, lightStateUpdate);

			var notFoundResult = result as NotFoundResult;
			Assert.NotNull(notFoundResult);
		}

		#endregion SetLightState

		#region GetLightScenes

		[Fact]
		public async Task GetLightScenes_Ok()
		{
			var hueLightProvider = Substitute.For<IHueLightProvider>();
			var lightSceneRepository = Substitute.For<IAsyncRepository<LightScene>>();
			IEnumerable<LightScene> lightScenes = new List<LightScene>()
			{
				new LightScene()
				{
					Id = 5,
					Name = "Test",
					Data = "abc"
				}
			};
			lightSceneRepository.GetAllAsync().Returns(Task.FromResult(lightScenes));
			var controller = new LightingController(hueLightProvider, lightSceneRepository);

			var result = await controller.GetLightScenes();

			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var scenes = Assert.IsType<List<LightScene>>(okResult.Value);
			Assert.Equal(lightScenes, scenes);
		}

		[Fact]
		public async Task GetLightScenes_NotFound()
		{
			var hueLightProvider = Substitute.For<IHueLightProvider>();
			var lightSceneRepository = Substitute.For<IAsyncRepository<LightScene>>();
			IEnumerable<LightScene> lightScenes = null;
			lightSceneRepository.GetAllAsync().Returns(Task.FromResult(lightScenes));
			var controller = new LightingController(hueLightProvider, lightSceneRepository);

			var result = await controller.GetLightScenes();

			Assert.IsType<NotFoundResult>(result.Result);
		}

		#endregion

		#region GetLightScene

		[Fact]
		public async Task GetLightScene_Ok()
		{
			var hueLightProvider = Substitute.For<IHueLightProvider>();
			var lightSceneRepository = Substitute.For<IAsyncRepository<LightScene>>();
			var lightScene = new LightScene()
			{
				Id = 5,
				Name = "Test",
				Data = "abc"
			};
			lightSceneRepository.GetAsync(5).Returns(Task.FromResult(lightScene));
			var controller = new LightingController(hueLightProvider, lightSceneRepository);

			var result = await controller.GetLightScene(5);

			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var scene = Assert.IsType<LightScene>(okResult.Value);
			Assert.Equal(5, scene.Id);
			Assert.Equal("Test", scene.Name);
			Assert.Equal("abc", scene.Data);
		}

		[Fact]
		public async Task GetLightScene_NotFound()
		{
			var hueLightProvider = Substitute.For<IHueLightProvider>();
			var lightSceneRepository = Substitute.For<IAsyncRepository<LightScene>>();
			LightScene lightScene = null;
			lightSceneRepository.GetAsync(5).Returns(Task.FromResult(lightScene));
			var controller = new LightingController(hueLightProvider, lightSceneRepository);

			var result = await controller.GetLightScene(2);

			Assert.IsType<NotFoundResult>(result.Result);
		}

		#endregion

		#region AddLightScene

		[Fact]
		public async Task AddLightScene_Created()
		{
			var hueLightProvider = Substitute.For<IHueLightProvider>();
			var lightSceneRepository = Substitute.For<IAsyncRepository<LightScene>>();
			var newLightScene = new LightScene()
			{
				Id = 5,
				Name = "Test",
				Data = "abc"
			};
			var controller = new LightingController(hueLightProvider, lightSceneRepository);

			var result = await controller.AddLightScene(newLightScene);

			await lightSceneRepository.Received(1).AddAsync(Arg.Is<LightScene>(x => x.Id.Equals(0) && x.Name.Equals("Test") && x.Data.Equals("abc")));
			var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
			var scene = Assert.IsType<LightScene>(createdResult.Value);
			Assert.Equal("Test", scene.Name);
			Assert.Equal("abc", scene.Data);
		}

		[Fact]
		public async Task AddLightScene_BadRequest()
		{
			var hueLightProvider = Substitute.For<IHueLightProvider>();
			var lightSceneRepository = Substitute.For<IAsyncRepository<LightScene>>();
			LightScene newLightScene = null;
			var controller = new LightingController(hueLightProvider, lightSceneRepository);

			var result = await controller.AddLightScene(newLightScene);

			Assert.IsType<BadRequestResult>(result.Result);
		}

		#endregion

		#region UpdateLightScene

		[Fact]
		public async Task UpdateLightScene_Ok()
		{
			var hueLightProvider = Substitute.For<IHueLightProvider>();
			var lightSceneRepository = Substitute.For<IAsyncRepository<LightScene>>();
			var lightScene = new LightScene()
			{
				Id = 5,
				Name = "Test",
				Data = "abc"
			};
			var controller = new LightingController(hueLightProvider, lightSceneRepository);

			var result = await controller.UpdateLightScene(5, lightScene);

			await lightSceneRepository.Received(1).UpdateAsync(Arg.Is<LightScene>(x => x.Id.Equals(5) && x.Name.Equals("Test") && x.Data.Equals("abc")));
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var scene = Assert.IsType<LightScene>(okResult.Value);
			Assert.Equal(5, scene.Id);
			Assert.Equal("Test", scene.Name);
			Assert.Equal("abc", scene.Data);
		}

		[Fact]
		public async Task UpdateLightScene_BadRequest_InvalidId()
		{
			var hueLightProvider = Substitute.For<IHueLightProvider>();
			var lightSceneRepository = Substitute.For<IAsyncRepository<LightScene>>();
			LightScene lightScene = new LightScene()
			{
				Id = 5,
				Name = "Test",
				Data = "abc"
			};
			var controller = new LightingController(hueLightProvider, lightSceneRepository);

			var result = await controller.UpdateLightScene(7, lightScene);

			Assert.IsType<BadRequestResult>(result.Result);
		}

		[Fact]
		public async Task UpdateLightScene_BadRequest_SceneNull()
		{
			var hueLightProvider = Substitute.For<IHueLightProvider>();
			var lightSceneRepository = Substitute.For<IAsyncRepository<LightScene>>();
			LightScene lightScene = null;
			var controller = new LightingController(hueLightProvider, lightSceneRepository);

			var result = await controller.UpdateLightScene(5, lightScene);

			Assert.IsType<BadRequestResult>(result.Result);
		}

		#endregion

		#region DeleteLightScene

		[Fact]
		public async Task DeleteLightScene_Ok()
		{
			var hueLightProvider = Substitute.For<IHueLightProvider>();
			var lightSceneRepository = Substitute.For<IAsyncRepository<LightScene>>();
			var lightScene = new LightScene()
			{
				Id = 5,
				Name = "Test",
				Data = "abc"
			};
			lightSceneRepository.GetAsync(Arg.Is<int>(5)).Returns(lightScene);
			var controller = new LightingController(hueLightProvider, lightSceneRepository);

			var result = await controller.DeleteLightScene(5);

			await lightSceneRepository.Received(1).RemoveAsync(Arg.Is<LightScene>(x => x.Id.Equals(5) && x.Name.Equals("Test") && x.Data.Equals("abc")));
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var scene = Assert.IsType<LightScene>(okResult.Value);
			Assert.Equal(5, scene.Id);
			Assert.Equal("Test", scene.Name);
			Assert.Equal("abc", scene.Data);
		}

		[Fact]
		public async Task DeleteLightScene_NotFound()
		{
			var hueLightProvider = Substitute.For<IHueLightProvider>();
			var lightSceneRepository = Substitute.For<IAsyncRepository<LightScene>>();
			LightScene lightScene = null;
			lightSceneRepository.GetAsync(Arg.Is<int>(5)).Returns(lightScene);
			var controller = new LightingController(hueLightProvider, lightSceneRepository);

			var result = await controller.DeleteLightScene(5);

			Assert.IsType<NotFoundResult>(result.Result);
		}

		#endregion

		#region ApplyLightScene

		[Fact]
		public async Task ApplyLightScene_Ok()
		{
			var hueLightProvider = Substitute.For<IHueLightProvider>();
			hueLightProvider.ApplyLightSceneAsync(Arg.Any<LightScene>()).Returns(true);
			var lightSceneRepository = Substitute.For<IAsyncRepository<LightScene>>();
			LightScene lightScene = new LightScene()
			{
				Id = 5,
				Name = "Test",
				Data = "abc"
			};
			lightSceneRepository.GetAsync(Arg.Is<int>(5)).Returns(lightScene);
			var controller = new LightingController(hueLightProvider, lightSceneRepository);

			var result = await controller.ApplyLightScene(5);

			await hueLightProvider.Received(1).ApplyLightSceneAsync(Arg.Is<LightScene>(x => x.Id.Equals(5) && x.Name.Equals("Test") && x.Data.Equals("abc")));
			Assert.IsType<OkResult>(result);
		}

		[Fact]
		public async Task ApplyLightScene_NotSuccessful()
		{
			var hueLightProvider = Substitute.For<IHueLightProvider>();
			hueLightProvider.ApplyLightSceneAsync(Arg.Any<LightScene>()).Returns(false);
			var lightSceneRepository = Substitute.For<IAsyncRepository<LightScene>>();
			LightScene lightScene = new LightScene()
			{
				Id = 5,
				Name = "Test",
				Data = "abc"
			};
			lightSceneRepository.GetAsync(Arg.Is<int>(5)).Returns(lightScene);
			var controller = new LightingController(hueLightProvider, lightSceneRepository);

			var result = await controller.ApplyLightScene(5);

			await hueLightProvider.Received(1).ApplyLightSceneAsync(Arg.Is<LightScene>(x => x.Id.Equals(5) && x.Name.Equals("Test") && x.Data.Equals("abc")));
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task ApplyLightScene_SceneNotFound()
		{
			var hueLightProvider = Substitute.For<IHueLightProvider>();
			var lightSceneRepository = Substitute.For<IAsyncRepository<LightScene>>();
			LightScene lightScene = null;
			lightSceneRepository.GetAsync(Arg.Is<int>(5)).Returns(lightScene);
			var controller = new LightingController(hueLightProvider, lightSceneRepository);

			var result = await controller.ApplyLightScene(5);

			Assert.IsType<NotFoundResult>(result);
		}

		#endregion
	}
}