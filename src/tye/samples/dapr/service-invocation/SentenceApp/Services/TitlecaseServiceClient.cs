using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Dapr.Client;
using Shared;

namespace SentenceApp.Services
{
    public class TitlecaseServiceClient
    {
        private readonly HttpClient _client;
        private readonly DaprClient _daprClient;

        public TitlecaseServiceClient(HttpClient client, DaprClient daprClient)
        {
            _client = client;
            _daprClient = daprClient;
        }

        private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public async Task<ConvertedResult> Convert(string sentence)
        {
            // Using Dapr sidecar and service invocation building block
            return await _daprClient.InvokeMethodAsync<object, ConvertedResult>("titlecaseservice", "titlecase", new object(), 
                HttpInvocationOptions.UsingGet()
                    .WithQueryParam("sentence", sentence));

            // If you're using Tye alone (without dapr)
            //var responseMessage = await _client.GetAsync($"/titlecase?sentence={sentence}");
            //var stream = await responseMessage.Content.ReadAsStreamAsync();
            //return await JsonSerializer.DeserializeAsync<ConvertedResult>(stream, _options);
        }
    }
}
