using System.Net;
using System.Net.Http.Json;
using Api.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Api.Tests.@public;

[Trait("Category", "public")]
public class BasicGrossProfitTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact(DisplayName = "GET /api/gross-profit returns 200")]
    public async Task GrossProfit_Endpoint_Responds_OK()
    {
        HttpResponseMessage response = await _client.GetAsync("/api/gross-profit?limit=5");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact(DisplayName = "Invalid sort returns 400")]
    public async Task Invalid_Sort_Returns_BadRequest()
    {
        HttpResponseMessage resp = await _client.GetAsync("/api/gross-profit?sort=sideways");
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
    }

    [Fact(DisplayName = "GET /api/gross-profit returns data")]
    public async Task Should_Fail_Until_Implemented()
    {
        ProductProfitDto[]? data = await _client.GetFromJsonAsync<ProductProfitDto[]>(
            "/api/gross-profit?limit=5"
        );
        Assert.NotNull(data);
        Assert.NotEmpty(data);
    }
}
