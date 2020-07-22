using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomeAPI.Backend.Controllers;
using HomeAPI.Backend.Models.Weather;
using HomeAPI.Backend.Models.Weather.OpenWeatherMap;
using HomeAPI.Backend.Providers.Weather;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace HomeAPI.Backend.Tests.Controllers
{
	public class WeatherControllerTests
	{
		[Fact]
		public async void GetWeather_Valid()
		{
			var owmProvider = Substitute.For<IOWMProvider>();
			var weather = new CurrentWeatherResponse()
			{
				Latitude = 0.123f,
				Longitude = 1.5f,
				Current = new CurrentWeatherData()
				{
					Timestamp = new DateTime(2020, 07, 22, 18, 47, 01, DateTimeKind.Utc),
					Sunrise = new DateTime(2020, 07, 22, 18, 47, 01, DateTimeKind.Utc),
					Sunset = new DateTime(2020, 07, 22, 18, 47, 01, DateTimeKind.Utc),
					Temperature = 20.3f,
					TemperatureFeelsLike = 19.72f,
					Pressure = 971,
					Humidity = 80,
					UVIndex = 4.7f,
					Clouds = 29,
					Visibility = 8000,
					WindSpeed = 100,
					WindDirection = 300,
					Weather = new WeatherCondition
					{
						Description = "Many clouds!",
						IconId = "10d"
					}
				},
				Daily = new List<DailyWeatherData>()
				{
					new DailyWeatherData()
					{
						Timestamp = new DateTime(2020, 07, 22, 18, 47, 01, DateTimeKind.Utc),
						Sunrise = new DateTime(2020, 07, 22, 18, 47, 01, DateTimeKind.Utc),
						Sunset = new DateTime(2020, 07, 22, 18, 47, 01, DateTimeKind.Utc),
						Temperature = new DayTemperatureSet()
						{
							TemperatureMorning = 1f,
							TemperatureDay = 2f,
							TemperatureEvening = 3f,
							TemperatureNight = 4f,
							TemperatureMin = 0.1f,
							TemperatureMax = 101.99f
						},
						TemperatureFeelsLike = new DayTemperatureSet()
						{
							TemperatureMorning = 1f,
							TemperatureDay = 2f,
							TemperatureEvening = 3f,
							TemperatureNight = 4f,
							TemperatureMin = 0.1f,
							TemperatureMax = 101.99f
						},
						Pressure = 971,
						Humidity = 80,
						UVIndex = 4.7f,
						Clouds = 29,
						WindSpeed = 100,
						WindDirection = 300,
						Weather = new WeatherCondition
						{
							Description = "Many clouds!",
							IconId = "10d"
						}
					}
				}
			};
			owmProvider.GetWeatherAsync().Returns(Task.FromResult(weather));
			var controller = new WeatherController(owmProvider);

			var result = await controller.GetWeather();

			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var resultWeather = Assert.IsType<CurrentWeatherResponse>(okResult.Value);
			Assert.Equal(weather, resultWeather);
		}

		[Fact]
		public async void GetWeather_DataNull()
		{
			var owmProvider = Substitute.For<IOWMProvider>();
			CurrentWeatherResponse weather = null;
			owmProvider.GetWeatherAsync().Returns(Task.FromResult(weather));
			var controller = new WeatherController(owmProvider);

			var result = await controller.GetWeather();

			Assert.IsType<NotFoundResult>(result.Result);
		}
	}
}