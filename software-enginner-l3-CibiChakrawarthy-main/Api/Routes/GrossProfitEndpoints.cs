using System.Data;
using Api.Models;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Api.Routes;

public static class GrossProfitEndpoints
{
    public static void MapGrossProfitEndpoints(this WebApplication app)
    {
        app.MapGet(
                "/api/gross-profit",
                async (int? limit, string? sort, DateOnly? minDate, DateOnly? maxDate, IDbConnection db) =>
                {
                    sort = sort?.ToLowerInvariant() ?? "desc";
                    if (sort != "asc" && sort != "desc")
                        return Results.BadRequest("Invalid sort parameter");
                    if (limit is not null && limit <= 0)
                        return Results.BadRequest("Limit must be positive");

                    StringBuilder sql = new();
                    sql.Append(@"SELECT p.Id AS ProductId,
                                       p.Name,
                                       p.UnitCost,
                                       p.UnitPrice,
                                       COALESCE(SUM((p.UnitPrice - p.UnitCost) * oi.Quantity),0) AS GrossProfit
                                FROM Products p
                                LEFT JOIN OrderItems oi ON oi.ProductId = p.Id
                                LEFT JOIN Orders o ON o.Id = oi.OrderId AND o.Status = 'Complete'");

                    List<string> conditions = new();
                    if (minDate.HasValue)
                        conditions.Add("o.OrderDate >= @MinDate");
                    if (maxDate.HasValue)
                        conditions.Add("o.OrderDate <= @MaxDate");
                    if (conditions.Count > 0)
                        sql.Append(" AND ").Append(string.Join(" AND ", conditions));

                    sql.Append(@" GROUP BY p.Id, p.Name, p.UnitCost, p.UnitPrice");
                    sql.Append($" ORDER BY GrossProfit {(sort == "asc" ? "ASC" : "DESC")}");
                    if (limit.HasValue)
                        sql.Append(" LIMIT @Limit");

                    var data = await db.QueryAsync<ProductProfitDto>(sql.ToString(), new
                    {
                        Limit = limit,
                        MinDate = minDate?.ToString("yyyy-MM-dd"),
                        MaxDate = maxDate?.ToString("yyyy-MM-dd")
                    });

                    return Results.Ok(data.ToList());
                }
            )
            .WithName("GetGrossProfit")
            .Produces<List<ProductProfitDto>>()
            .Produces(StatusCodes.Status400BadRequest);
    }
}
