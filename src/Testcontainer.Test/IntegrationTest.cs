using System.Net;
using FluentAssertions;
using Testcontainer.Api.Clients.BlogClient;
using Testcontainer.Test.Infrastructure;

namespace Testcontainer.Test;

public class IntegrationTest : IntegrationBase, IDisposable
{
    private readonly BlogFixture _blogFixture;

    public IntegrationTest(ApiWebFactory<Program> factory) : base(factory)
    {
        _blogFixture = new BlogFixture();
    }
    
    public void Dispose()
    {
        _blogFixture.Reset();
        _blogFixture.Dispose();
    }

    [Fact]
    public async Task Given_weather_api_successful_returns_weather()
    {
        // Arrange
        _blogFixture.SetupGetBlog("Resources/success.json");

        // Act
        var request = CreateGetRequest("/posts");
        var result = await HttpClient.SendAsync(request);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await ReadResponseAsync<IEnumerable<Post>>(result);
        response.Should().HaveCount(2);

        response.Should().Contain(x =>
            x.Title == "sunt aut facere repellat provident occaecati excepturi optio reprehenderit");
    }
}
