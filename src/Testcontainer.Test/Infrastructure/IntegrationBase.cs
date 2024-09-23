using Newtonsoft.Json;

namespace Testcontainer.Test.Infrastructure;

[Collection("sequential")]
public abstract class IntegrationBase : IClassFixture<ApiWebFactory<Program>>
{
    protected HttpClient HttpClient { get; }
    
    protected IntegrationBase(ApiWebFactory<Program> factory)
    {
        HttpClient = factory.CreateClient();
        factory.Server.AllowSynchronousIO = true;
    }

    protected HttpRequestMessage CreateGetRequest(string url)
    {
        return new HttpRequestMessage(HttpMethod.Get, url);
    }

    protected static async Task<T> ReadResponseAsync<T>(HttpResponseMessage response)
    {
        var result = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(result);
    }
}
