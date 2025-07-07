namespace Api.Models;

public record ProductProfitDto(
    long ProductId,
    string Name,
    double UnitCost,
    double UnitPrice,
    double GrossProfit
);
