using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Testcontainer.Test.Infrastructure;

public class BlogFixture : IDisposable
{
    protected readonly WireMockServer _mockApi;

    public BlogFixture()
    {
        _mockApi = WireMockServer.Start(50000);
    }
    
    public void Dispose()
    {
        _mockApi.Stop();
    }

    public void Reset()
    {
        _mockApi.Reset();
    }

    public IRequestBuilder SetupGetBlog(string responseBodyResource, int statusCode = 200)
    {
        var request = Request.Create()
            .UsingGet()
            .WithPath("/posts*");

        var responseBody = string.IsNullOrWhiteSpace(responseBodyResource) ? new byte[0] : File.ReadAllBytes(responseBodyResource);

        _mockApi.Given(request)
            .RespondWith(
                Response.Create()
                .WithStatusCode(statusCode)
                .WithHeader("content-type", "application/json")
                .WithBody(responseBody)
            );

        return request;
    }
}
