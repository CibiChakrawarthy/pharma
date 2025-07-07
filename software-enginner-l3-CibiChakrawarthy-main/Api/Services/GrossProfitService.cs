using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using Api.Models;

namespace Api.Services;

public class GrossProfitService : IGrossProfitService
{
    private readonly IDbConnection _db;

    public GrossProfitService(IDbConnection db)
    {
        _db = db;
    }

    public async Task<List<ProductProfitDto>> GetGrossProfitAsync(int? limit, string sort, DateOnly? minDate, DateOnly? maxDate)
    {
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

        var data = await _db.QueryAsync<ProductProfitDto>(sql.ToString(), new
        {
            Limit = limit,
            MinDate = minDate?.ToString("yyyy-MM-dd"),
            MaxDate = maxDate?.ToString("yyyy-MM-dd")
        });

        return data.ToList();
    }
}
