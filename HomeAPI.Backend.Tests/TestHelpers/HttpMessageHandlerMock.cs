using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HomeAPI.Backend.Tests.TestHelpers
{
	public class HttpMessageHandlerMock : HttpMessageHandler
	{
		private readonly string response;
		private readonly HttpStatusCode statusCode;

		public string Input { get; private set; }

		public int NumberOfCalls { get; private set; }

		public HttpMessageHandlerMock(string response, HttpStatusCode statusCode)
		{
			this.response = response;
			this.statusCode = statusCode;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			NumberOfCalls++;

			if (request.Content != null)
			{
				Input = await request.Content.ReadAsStringAsync();
			}

			return new HttpResponseMessage()
			{
				StatusCode = statusCode,
				Content = new StringContent(response)
			};
		}
	}
}