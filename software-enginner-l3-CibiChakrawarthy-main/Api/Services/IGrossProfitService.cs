using Api.Models;

namespace Api.Services;

public interface IGrossProfitService
{
    Task<List<ProductProfitDto>> GetGrossProfitAsync(int? limit, string sort, DateOnly? minDate, DateOnly? maxDate);
}
