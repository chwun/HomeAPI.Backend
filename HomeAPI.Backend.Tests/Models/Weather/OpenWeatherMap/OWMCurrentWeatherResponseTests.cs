using System;
using HomeAPI.Backend.Models.Weather.OpenWeatherMap;
using Xunit;

namespace HomeAPI.Backend.Tests.Models.Weather.OpenWeatherMap
{
	public class OWMCurrentWeatherResponseTests
	{
		[Fact]
		public void ToCurrentWeatherResponse()
		{
			var owmWeatherResponse = new OWMCurrentWeatherResponse()
			{
				Lat = 0.123f,
				Lon = 1.5f,
				Current = new OWMCurrentWeatherData()
				{
					Dt = ((DateTimeOffset)new DateTime(2020, 07, 22, 18, 47, 01, DateTimeKind.Utc)).ToUnixTimeSeconds(),
					Sunrise = ((DateTimeOffset)new DateTime(2020, 07, 22, 18, 48, 01, DateTimeKind.Utc)).ToUnixTimeSeconds(),
					Sunset = ((DateTimeOffset)new DateTime(2020, 07, 22, 18, 49, 01, DateTimeKind.Utc)).ToUnixTimeSeconds(),
					Temp = 20.3f,
					Feels_Like = 19.72f,
					Pressure = 971,
					Humidity = 80,
					Uvi = 4.7f,
					Clouds = 29,
					Visibility = 8000,
					Wind_Speed = 100,
					Wind_Deg = 300,
					Weather = new OWMWeatherCondition[]
					{
						new OWMWeatherCondition()
						{
							Id = 5,
							Main = "Clouds",
							Description = "Many clouds!",
							Icon = "10d"
						}
					}
				},
				Daily = new OWMDailyWeatherData[]{
					new OWMDailyWeatherData()
					{
						Dt = ((DateTimeOffset)new DateTime(2020, 07, 22, 18, 47, 01, DateTimeKind.Utc)).ToUnixTimeSeconds(),
						Sunrise = ((DateTimeOffset)new DateTime(2020, 07, 22, 18, 48, 01, DateTimeKind.Utc)).ToUnixTimeSeconds(),
						Sunset = ((DateTimeOffset)new DateTime(2020, 07, 22, 18, 49, 01, DateTimeKind.Utc)).ToUnixTimeSeconds(),
						Temp = new OWMDayTemperatureSet()
						{
							Morn = 1f,
							Day = 2f,
							Eve = 3f,
							Night = 4f,
							Min = 0.1f,
							Max = 101.99f
						},
						Feels_Like = new OWMDayTemperatureSet()
						{
							Morn = 10f,
							Day = 20f,
							Eve = 30f,
							Night = 40f,
							Min = 0.01f,
							Max = 1010.99f
						},
						Pressure = 971,
						Humidity = 80,
						Uvi = 4.7f,
						Clouds = 29,
						Wind_Speed = 100,
						Wind_Deg = 300,
						Weather = new OWMWeatherCondition[]
						{
							new OWMWeatherCondition()
							{
								Id = 5,
								Main = "Clouds",
								Description = "Many clouds!",
								Icon = "10d"
							}
						}
					},
					new OWMDailyWeatherData()
					{
						Dt = ((DateTimeOffset)new DateTime(2020, 07, 22, 18, 47, 01, DateTimeKind.Utc)).ToUnixTimeSeconds(),
						Sunrise = ((DateTimeOffset)new DateTime(2020, 07, 22, 18, 48, 01, DateTimeKind.Utc)).ToUnixTimeSeconds(),
						Sunset = ((DateTimeOffset)new DateTime(2020, 07, 22, 18, 49, 01, DateTimeKind.Utc)).ToUnixTimeSeconds(),
						Temp = new OWMDayTemperatureSet()
						{
							Morn = 1f,
							Day = 2f,
							Eve = 3f,
							Night = 4f,
							Min = 0.1f,
							Max = 101.99f
						},
						Feels_Like = new OWMDayTemperatureSet()
						{
							Morn = 10f,
							Day = 20f,
							Eve = 30f,
							Night = 40f,
							Min = 0.01f,
							Max = 1010.99f
						},
						Pressure = 971,
						Humidity = 80,
						Uvi = 4.7f,
						Clouds = 29,
						Wind_Speed = 100,
						Wind_Deg = 300,
						Weather = new OWMWeatherCondition[]
						{
							new OWMWeatherCondition()
							{
								Id = 5,
								Main = "Clouds",
								Description = "Many clouds!",
								Icon = "10d"
							}
						}
					}
				}
			};

			var result = owmWeatherResponse.ToCurrentWeatherResponse();

			Assert.Equal(0.123f, result.Latitude);
			Assert.Equal(1.5, result.Longitude);
			Assert.Equal(new DateTime(2020, 07, 22, 18, 47, 01, DateTimeKind.Utc), result.Current.Timestamp);
			Assert.Equal(new DateTime(2020, 07, 22, 18, 48, 01, DateTimeKind.Utc), result.Current.Sunrise);
			Assert.Equal(new DateTime(2020, 07, 22, 18, 49, 01, DateTimeKind.Utc), result.Current.Sunset);
			Assert.Equal(20.3f, result.Current.Temperature);
			Assert.Equal(19.72f, result.Current.TemperatureFeelsLike);
			Assert.Equal(971, result.Current.Pressure);
			Assert.Equal(80, result.Current.Humidity);
			Assert.Equal(4.7f, result.Current.UVIndex);
			Assert.Equal(29, result.Current.Clouds);
			Assert.Equal(8000, result.Current.Visibility);
			Assert.Equal(100, result.Current.WindSpeed);
			Assert.Equal(300, result.Current.WindDirection);
			Assert.Equal("Many clouds!", result.Current.Weather.Description);
			Assert.Equal("10d", result.Current.Weather.IconId);

			Assert.Equal(2, result.Daily.Count);

			Assert.Equal(new DateTime(2020, 07, 22, 18, 47, 01, DateTimeKind.Utc), result.Daily[0].Timestamp);
			Assert.Equal(new DateTime(2020, 07, 22, 18, 48, 01, DateTimeKind.Utc), result.Daily[0].Sunrise);
			Assert.Equal(new DateTime(2020, 07, 22, 18, 49, 01, DateTimeKind.Utc), result.Daily[0].Sunset);
			Assert.Equal(1f, result.Daily[0].Temperature.TemperatureMorning);
			Assert.Equal(2f, result.Daily[0].Temperature.TemperatureDay);
			Assert.Equal(3f, result.Daily[0].Temperature.TemperatureEvening);
			Assert.Equal(4f, result.Daily[0].Temperature.TemperatureNight);
			Assert.Equal(0.1f, result.Daily[0].Temperature.TemperatureMin);
			Assert.Equal(101.99f, result.Daily[0].Temperature.TemperatureMax);
			Assert.Equal(10f, result.Daily[0].TemperatureFeelsLike.TemperatureMorning);
			Assert.Equal(20f, result.Daily[0].TemperatureFeelsLike.TemperatureDay);
			Assert.Equal(30f, result.Daily[0].TemperatureFeelsLike.TemperatureEvening);
			Assert.Equal(40f, result.Daily[0].TemperatureFeelsLike.TemperatureNight);
			Assert.Equal(0.01f, result.Daily[0].TemperatureFeelsLike.TemperatureMin);
			Assert.Equal(1010.99f, result.Daily[0].TemperatureFeelsLike.TemperatureMax);
			Assert.Equal(971, result.Daily[0].Pressure);
			Assert.Equal(80, result.Daily[0].Humidity);
			Assert.Equal(4.7f, result.Daily[0].UVIndex);
			Assert.Equal(29, result.Daily[0].Clouds);
			Assert.Equal(100, result.Daily[0].WindSpeed);
			Assert.Equal(300, result.Daily[0].WindDirection);
			Assert.Equal("Many clouds!", result.Daily[0].Weather.Description);
			Assert.Equal("10d", result.Daily[0].Weather.IconId);

			Assert.Equal(new DateTime(2020, 07, 22, 18, 47, 01, DateTimeKind.Utc), result.Daily[1].Timestamp);
			Assert.Equal(new DateTime(2020, 07, 22, 18, 48, 01, DateTimeKind.Utc), result.Daily[1].Sunrise);
			Assert.Equal(new DateTime(2020, 07, 22, 18, 49, 01, DateTimeKind.Utc), result.Daily[1].Sunset);
			Assert.Equal(1f, result.Daily[1].Temperature.TemperatureMorning);
			Assert.Equal(2f, result.Daily[1].Temperature.TemperatureDay);
			Assert.Equal(3f, result.Daily[1].Temperature.TemperatureEvening);
			Assert.Equal(4f, result.Daily[1].Temperature.TemperatureNight);
			Assert.Equal(0.1f, result.Daily[1].Temperature.TemperatureMin);
			Assert.Equal(101.99f, result.Daily[1].Temperature.TemperatureMax);
			Assert.Equal(10f, result.Daily[1].TemperatureFeelsLike.TemperatureMorning);
			Assert.Equal(20f, result.Daily[1].TemperatureFeelsLike.TemperatureDay);
			Assert.Equal(30f, result.Daily[1].TemperatureFeelsLike.TemperatureEvening);
			Assert.Equal(40f, result.Daily[1].TemperatureFeelsLike.TemperatureNight);
			Assert.Equal(0.01f, result.Daily[1].TemperatureFeelsLike.TemperatureMin);
			Assert.Equal(1010.99f, result.Daily[1].TemperatureFeelsLike.TemperatureMax);
			Assert.Equal(971, result.Daily[1].Pressure);
			Assert.Equal(80, result.Daily[1].Humidity);
			Assert.Equal(4.7f, result.Daily[1].UVIndex);
			Assert.Equal(29, result.Daily[1].Clouds);
			Assert.Equal(100, result.Daily[1].WindSpeed);
			Assert.Equal(300, result.Daily[1].WindDirection);
			Assert.Equal("Many clouds!", result.Daily[1].Weather.Description);
			Assert.Equal("10d", result.Daily[1].Weather.IconId);
		}
	}
}