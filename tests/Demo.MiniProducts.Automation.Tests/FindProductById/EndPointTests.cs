using System.Net;
using Demo.MiniProducts.Api.Features.FindById;
using FluentAssertions;
using Newtonsoft.Json;

namespace Demo.MiniProducts.Automation.Tests.FindProductById;

public class EndPointTests : TestBase
{
    private const string Products = "/products";

    public EndPointTests(TestWebApplicationFactory<Program> startUp) : base(startUp) { }

    [Fact(DisplayName = "Product exists")]
    public async Task ProductExists()
    {
        var response = await GetAsync<ProductResponse>(
            $"{Products}/tech/prod-1",
            Array.Empty<(string, string)>
        );

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var productResponse = JsonConvert.DeserializeObject<ProductResponse>(
            await response.Content.ReadAsStringAsync()
        );
        productResponse.Should().NotBeNull();
        productResponse.Data.Should().NotBeNull();
        productResponse.Data.Id.Should().Be("PROD-1");
    }

    [Fact(DisplayName = "Product does not exist")]
    public async Task ProductDoesNotExist()
    {
        var response = await GetAsync<ProductResponse>(
            $"{Products}/tech/blah",
            Array.Empty<(string, string)>
        );

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}