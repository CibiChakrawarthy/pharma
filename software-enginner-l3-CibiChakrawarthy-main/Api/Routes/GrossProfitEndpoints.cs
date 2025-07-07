using Api.Models;
using Api.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Api.Routes;

public static class GrossProfitEndpoints
{
    public static void MapGrossProfitEndpoints(this WebApplication app)
    {
        app.MapGet(
                "/api/gross-profit",
                async (int? limit, string? sort, DateOnly? minDate, DateOnly? maxDate, IGrossProfitService service) =>
                {
                    sort = sort?.ToLowerInvariant() ?? "desc";
                    if (sort != "asc" && sort != "desc")
                        return Results.BadRequest("Invalid sort parameter");
                    if (limit is not null && limit <= 0)
                        return Results.BadRequest("Limit must be positive");

                    var data = await service.GetGrossProfitAsync(limit, sort, minDate, maxDate);
                    return Results.Ok(data);
                }
            )
            .WithName("GetGrossProfit")
            .Produces<List<ProductProfitDto>>()
            .Produces(StatusCodes.Status400BadRequest);
    }
}
