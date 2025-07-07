using System.Data;
using Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Api.Routes;

public static class GrossProfitEndpoints
{
    public static void MapGrossProfitEndpoints(this WebApplication app)
    {
        app.MapGet(
                "/api/gross-profit",
                (int? limit, string? sort, IDbConnection db) =>
                    Results.Ok(new List<ProductProfitDto>())
            )
            .WithName("GetGrossProfit")
            .Produces<List<ProductProfitDto>>()
            .Produces(StatusCodes.Status400BadRequest);
    }
}
