using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Intcomex.ProductsApi.API;

namespace Intcomex.ProductsApi.Tests.Integration
{
    public class ProductIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllProducts_ReturnsOk()
        {
            var response = await _client.GetAsync("/Category");
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            body.Should().NotBeNullOrWhiteSpace();
        }
    }
}