using System;
using System.Net;
using System.Net.Http;
using HomeAPI.Backend.Common;
using HomeAPI.Backend.Options;
using HomeAPI.Backend.Providers.Weather;
using HomeAPI.Backend.Tests.TestHelpers;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace HomeAPI.Backend.Tests.Providers.Weather
{
	public class OWMProviderTests
	{
#region constructor

		[Fact]
		public void OWMProvider_Constructor()
		{
			var clientFactory = Substitute.For<IHttpClientFactory>();
			var optionsMonitor = Substitute.For<IOptionsMonitor<OWMOptions>>();
			var owmOptions = new OWMOptions()
			{
				Latitude = 20.1f,
				Longitude = 10.07f,
				LanguageCode = "de",
				ApiKey = "xyz123"
			};
			optionsMonitor.CurrentValue.Returns(owmOptions);
			var dateTimeProvider = Substitute.For<IDateTimeProvider>();
			dateTimeProvider.UtcNow.Returns(new DateTime(2020, 07, 22));

			var owmProvider = new OWMProvider(clientFactory, optionsMonitor, dateTimeProvider);

			Assert.NotNull(owmProvider); // dummy test
		}

		#endregion

		#region GetWeatherAsync

		[Fact]
		public async void GetWeatherAsync_InvalidJson()
		{
			string response = "{abc}";
			HttpStatusCode statusCode = HttpStatusCode.OK;
			var messageHandlerMock = new HttpMessageHandlerMock(response, statusCode);
			var httpClient = new HttpClient(messageHandlerMock);
			var clientFactory = Substitute.For<IHttpClientFactory>();
			clientFactory.CreateClient().Returns(httpClient);
			var optionsMonitor = Substitute.For<IOptionsMonitor<OWMOptions>>();
			var owmOptions = new OWMOptions()
			{
				Latitude = 20.1f,
				Longitude = 10.07f,
				LanguageCode = "de",
				ApiKey = "xyz123"
			};
			optionsMonitor.CurrentValue.Returns(owmOptions);
			var dateTimeProvider = Substitute.For<IDateTimeProvider>();
			dateTimeProvider.UtcNow.Returns(new DateTime(2020, 07, 22));
			var owmProvider = new OWMProvider(clientFactory, optionsMonitor, dateTimeProvider);

			var result = await owmProvider.GetWeatherAsync();

			Assert.Null(result);
		}

		[Fact]
		public async void GetWeatherAsync_Successful()
		{
			string response = "{\"lat\":30,\"lon\":10,\"timezone\":\"Africa/Tripoli\",\"timezone_offset\":7200,\"current\":{\"dt\":1595439272,\"sunrise\":1595392384,\"sunset\":1595441970,\"temp\":34.22,\"feels_like\":27.62,\"pressure\":1013,\"humidity\":18,\"dew_point\":6.53,\"uvi\":12.77,\"clouds\":0,\"visibility\":10000,\"wind_speed\":8.26,\"wind_deg\":26,\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"Klarer Himmel\",\"icon\":\"01d\"}]},\"daily\":[{\"dt\":1595415600,\"sunrise\":1595392384,\"sunset\":1595441970,\"temp\":{\"day\":34.22,\"min\":26.44,\"max\":34.22,\"night\":26.44,\"eve\":34.22,\"morn\":34.22},\"feels_like\":{\"day\":28.14,\"night\":22.29,\"eve\":27.62,\"morn\":28.14},\"pressure\":1013,\"humidity\":18,\"dew_point\":6.53,\"wind_speed\":7.52,\"wind_deg\":23,\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"Klarer Himmel\",\"icon\":\"01d\"}],\"clouds\":0,\"pop\":0,\"uvi\":12.77},{\"dt\":1595502000,\"sunrise\":1595478820,\"sunset\":1595528339,\"temp\":{\"day\":35.01,\"min\":23.2,\"max\":37.31,\"night\":26.11,\"eve\":33.91,\"morn\":23.2},\"feels_like\":{\"day\":30.5,\"night\":22.21,\"eve\":29.26,\"morn\":20.43},\"pressure\":1011,\"humidity\":12,\"dew_point\":2.35,\"wind_speed\":3.9,\"wind_deg\":64,\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"Klarer Himmel\",\"icon\":\"01d\"}],\"clouds\":0,\"pop\":0,\"uvi\":13.3},{\"dt\":1595588400,\"sunrise\":1595565255,\"sunset\":1595614707,\"temp\":{\"day\":37.22,\"min\":24.76,\"max\":39.01,\"night\":28.69,\"eve\":34.81,\"morn\":24.76},\"feels_like\":{\"day\":33.71,\"night\":22.56,\"eve\":30.13,\"morn\":20.96},\"pressure\":1010,\"humidity\":9,\"dew_point\":0.52,\"wind_speed\":1.98,\"wind_deg\":88,\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"Klarer Himmel\",\"icon\":\"01d\"}],\"clouds\":0,\"pop\":0,\"uvi\":12.64},{\"dt\":1595674800,\"sunrise\":1595651691,\"sunset\":1595701073,\"temp\":{\"day\":38.63,\"min\":26.36,\"max\":39.83,\"night\":29.93,\"eve\":34.94,\"morn\":26.36},\"feels_like\":{\"day\":33.68,\"night\":24.32,\"eve\":30.46,\"morn\":22.39},\"pressure\":1011,\"humidity\":8,\"dew_point\":-0.58,\"wind_speed\":3.93,\"wind_deg\":87,\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"Klarer Himmel\",\"icon\":\"01d\"}],\"clouds\":0,\"pop\":0,\"uvi\":13.2},{\"dt\":1595761200,\"sunrise\":1595738128,\"sunset\":1595787438,\"temp\":{\"day\":39.63,\"min\":27.63,\"max\":40.51,\"night\":30.03,\"eve\":36.11,\"morn\":27.63},\"feels_like\":{\"day\":33.59,\"night\":23.84,\"eve\":31.81,\"morn\":24.4},\"pressure\":1012,\"humidity\":10,\"dew_point\":2.83,\"wind_speed\":6.31,\"wind_deg\":93,\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"Klarer Himmel\",\"icon\":\"01d\"}],\"clouds\":0,\"pop\":0,\"uvi\":13.05},{\"dt\":1595847600,\"sunrise\":1595824564,\"sunset\":1595873802,\"temp\":{\"day\":39.41,\"min\":27.45,\"max\":41.11,\"night\":29.04,\"eve\":37.41,\"morn\":27.45},\"feels_like\":{\"day\":36.17,\"night\":26.15,\"eve\":33.01,\"morn\":23.42},\"pressure\":1013,\"humidity\":11,\"dew_point\":4.32,\"wind_speed\":2.61,\"wind_deg\":68,\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"Klarer Himmel\",\"icon\":\"01d\"}],\"clouds\":0,\"pop\":0,\"uvi\":12.29},{\"dt\":1595934000,\"sunrise\":1595911000,\"sunset\":1595960164,\"temp\":{\"day\":39.51,\"min\":27.61,\"max\":41.21,\"night\":27.61,\"eve\":36.61,\"morn\":27.91},\"feels_like\":{\"day\":34.64,\"night\":24.03,\"eve\":31.44,\"morn\":25.61},\"pressure\":1010,\"humidity\":11,\"dew_point\":4.42,\"wind_speed\":4.95,\"wind_deg\":28,\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"Klarer Himmel\",\"icon\":\"01d\"}],\"clouds\":0,\"pop\":0,\"uvi\":12.03},{\"dt\":1596020400,\"sunrise\":1595997437,\"sunset\":1596046526,\"temp\":{\"day\":40.42,\"min\":27.71,\"max\":41.71,\"night\":30.01,\"eve\":37.32,\"morn\":27.71},\"feels_like\":{\"day\":37.37,\"night\":25.72,\"eve\":31.81,\"morn\":23.49},\"pressure\":1008,\"humidity\":8,\"dew_point\":1.32,\"wind_speed\":1.47,\"wind_deg\":52,\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"Klarer Himmel\",\"icon\":\"01d\"}],\"clouds\":0,\"pop\":0,\"uvi\":12.25}]}";
			HttpStatusCode statusCode = HttpStatusCode.OK;
			var messageHandlerMock = new HttpMessageHandlerMock(response, statusCode);
			var httpClient = new HttpClient(messageHandlerMock);
			var clientFactory = Substitute.For<IHttpClientFactory>();
			clientFactory.CreateClient().Returns(httpClient);
			var optionsMonitor = Substitute.For<IOptionsMonitor<OWMOptions>>();
			var owmOptions = new OWMOptions()
			{
				Latitude = 30.0f,
				Longitude = 10.0f,
				LanguageCode = "de",
				ApiKey = "xyz123"
			};
			optionsMonitor.CurrentValue.Returns(owmOptions);
			var dateTimeProvider = Substitute.For<IDateTimeProvider>();
			dateTimeProvider.UtcNow.Returns(new DateTime(2020, 07, 22));
			var owmProvider = new OWMProvider(clientFactory, optionsMonitor, dateTimeProvider);

			var result = await owmProvider.GetWeatherAsync();

			Assert.NotNull(result);
		}

		#endregion

		#region GetDailyForecastAsync
		
		[Fact]
		public async void GetDailyForecastAsync_InvalidJson()
		{
			string response = "{abc}";
			HttpStatusCode statusCode = HttpStatusCode.OK;
			var messageHandlerMock = new HttpMessageHandlerMock(response, statusCode);
			var httpClient = new HttpClient(messageHandlerMock);
			var clientFactory = Substitute.For<IHttpClientFactory>();
			clientFactory.CreateClient().Returns(httpClient);
			var optionsMonitor = Substitute.For<IOptionsMonitor<OWMOptions>>();
			var owmOptions = new OWMOptions()
			{
				Latitude = 20.1f,
				Longitude = 10.07f,
				LanguageCode = "de",
				ApiKey = "xyz123"
			};
			optionsMonitor.CurrentValue.Returns(owmOptions);
			var dateTimeProvider = Substitute.For<IDateTimeProvider>();
			dateTimeProvider.UtcNow.Returns(new DateTime(2020, 07, 22));
			var owmProvider = new OWMProvider(clientFactory, optionsMonitor, dateTimeProvider);

			var result = await owmProvider.GetDailyForecastAsync();

			Assert.Null(result);
		}

		[Fact]
		public async void GetDailyForecastAsync_Successful()
		{
			string response = "{\"lat\":30,\"lon\":10,\"timezone\":\"Africa/Tripoli\",\"timezone_offset\":7200,\"current\":{\"dt\":1595439272,\"sunrise\":1595392384,\"sunset\":1595441970,\"temp\":34.22,\"feels_like\":27.62,\"pressure\":1013,\"humidity\":18,\"dew_point\":6.53,\"uvi\":12.77,\"clouds\":0,\"visibility\":10000,\"wind_speed\":8.26,\"wind_deg\":26,\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"Klarer Himmel\",\"icon\":\"01d\"}]},\"daily\":[{\"dt\":1595415600,\"sunrise\":1595392384,\"sunset\":1595441970,\"temp\":{\"day\":34.22,\"min\":26.44,\"max\":34.22,\"night\":26.44,\"eve\":34.22,\"morn\":34.22},\"feels_like\":{\"day\":28.14,\"night\":22.29,\"eve\":27.62,\"morn\":28.14},\"pressure\":1013,\"humidity\":18,\"dew_point\":6.53,\"wind_speed\":7.52,\"wind_deg\":23,\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"Klarer Himmel\",\"icon\":\"01d\"}],\"clouds\":0,\"pop\":0,\"uvi\":12.77},{\"dt\":1595502000,\"sunrise\":1595478820,\"sunset\":1595528339,\"temp\":{\"day\":35.01,\"min\":23.2,\"max\":37.31,\"night\":26.11,\"eve\":33.91,\"morn\":23.2},\"feels_like\":{\"day\":30.5,\"night\":22.21,\"eve\":29.26,\"morn\":20.43},\"pressure\":1011,\"humidity\":12,\"dew_point\":2.35,\"wind_speed\":3.9,\"wind_deg\":64,\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"Klarer Himmel\",\"icon\":\"01d\"}],\"clouds\":0,\"pop\":0,\"uvi\":13.3},{\"dt\":1595588400,\"sunrise\":1595565255,\"sunset\":1595614707,\"temp\":{\"day\":37.22,\"min\":24.76,\"max\":39.01,\"night\":28.69,\"eve\":34.81,\"morn\":24.76},\"feels_like\":{\"day\":33.71,\"night\":22.56,\"eve\":30.13,\"morn\":20.96},\"pressure\":1010,\"humidity\":9,\"dew_point\":0.52,\"wind_speed\":1.98,\"wind_deg\":88,\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"Klarer Himmel\",\"icon\":\"01d\"}],\"clouds\":0,\"pop\":0,\"uvi\":12.64},{\"dt\":1595674800,\"sunrise\":1595651691,\"sunset\":1595701073,\"temp\":{\"day\":38.63,\"min\":26.36,\"max\":39.83,\"night\":29.93,\"eve\":34.94,\"morn\":26.36},\"feels_like\":{\"day\":33.68,\"night\":24.32,\"eve\":30.46,\"morn\":22.39},\"pressure\":1011,\"humidity\":8,\"dew_point\":-0.58,\"wind_speed\":3.93,\"wind_deg\":87,\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"Klarer Himmel\",\"icon\":\"01d\"}],\"clouds\":0,\"pop\":0,\"uvi\":13.2},{\"dt\":1595761200,\"sunrise\":1595738128,\"sunset\":1595787438,\"temp\":{\"day\":39.63,\"min\":27.63,\"max\":40.51,\"night\":30.03,\"eve\":36.11,\"morn\":27.63},\"feels_like\":{\"day\":33.59,\"night\":23.84,\"eve\":31.81,\"morn\":24.4},\"pressure\":1012,\"humidity\":10,\"dew_point\":2.83,\"wind_speed\":6.31,\"wind_deg\":93,\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"Klarer Himmel\",\"icon\":\"01d\"}],\"clouds\":0,\"pop\":0,\"uvi\":13.05},{\"dt\":1595847600,\"sunrise\":1595824564,\"sunset\":1595873802,\"temp\":{\"day\":39.41,\"min\":27.45,\"max\":41.11,\"night\":29.04,\"eve\":37.41,\"morn\":27.45},\"feels_like\":{\"day\":36.17,\"night\":26.15,\"eve\":33.01,\"morn\":23.42},\"pressure\":1013,\"humidity\":11,\"dew_point\":4.32,\"wind_speed\":2.61,\"wind_deg\":68,\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"Klarer Himmel\",\"icon\":\"01d\"}],\"clouds\":0,\"pop\":0,\"uvi\":12.29},{\"dt\":1595934000,\"sunrise\":1595911000,\"sunset\":1595960164,\"temp\":{\"day\":39.51,\"min\":27.61,\"max\":41.21,\"night\":27.61,\"eve\":36.61,\"morn\":27.91},\"feels_like\":{\"day\":34.64,\"night\":24.03,\"eve\":31.44,\"morn\":25.61},\"pressure\":1010,\"humidity\":11,\"dew_point\":4.42,\"wind_speed\":4.95,\"wind_deg\":28,\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"Klarer Himmel\",\"icon\":\"01d\"}],\"clouds\":0,\"pop\":0,\"uvi\":12.03},{\"dt\":1596020400,\"sunrise\":1595997437,\"sunset\":1596046526,\"temp\":{\"day\":40.42,\"min\":27.71,\"max\":41.71,\"night\":30.01,\"eve\":37.32,\"morn\":27.71},\"feels_like\":{\"day\":37.37,\"night\":25.72,\"eve\":31.81,\"morn\":23.49},\"pressure\":1008,\"humidity\":8,\"dew_point\":1.32,\"wind_speed\":1.47,\"wind_deg\":52,\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"Klarer Himmel\",\"icon\":\"01d\"}],\"clouds\":0,\"pop\":0,\"uvi\":12.25}]}";
			HttpStatusCode statusCode = HttpStatusCode.OK;
			var messageHandlerMock = new HttpMessageHandlerMock(response, statusCode);
			var httpClient = new HttpClient(messageHandlerMock);
			var clientFactory = Substitute.For<IHttpClientFactory>();
			clientFactory.CreateClient().Returns(httpClient);
			var optionsMonitor = Substitute.For<IOptionsMonitor<OWMOptions>>();
			var owmOptions = new OWMOptions()
			{
				Latitude = 30.0f,
				Longitude = 10.0f,
				LanguageCode = "de",
				ApiKey = "xyz123"
			};
			optionsMonitor.CurrentValue.Returns(owmOptions);
			var dateTimeProvider = Substitute.For<IDateTimeProvider>();
			dateTimeProvider.UtcNow.Returns(new DateTime(2020, 07, 22));
			var owmProvider = new OWMProvider(clientFactory, optionsMonitor, dateTimeProvider);

			var result = await owmProvider.GetDailyForecastAsync();

			Assert.Equal(7, result.Count);
		}

		#endregion
	}
}