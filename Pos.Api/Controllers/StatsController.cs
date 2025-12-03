    using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pos.Api.Data;

namespace Pos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly PosDbContext _db;

        public StatsController(PosDbContext db)
        {
            _db = db;
        }

        // GET /api/stats/summary?locationCode=MAIN
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary([FromQuery] string? locationCode = null)
        {
            var today = DateTime.UtcNow.Date;

            var query = _db.Orders
                .Include(o => o.Location)
                .Where(o => o.CreatedAtUtc >= today &&
                            o.CreatedAtUtc < today.AddDays(1));

            if (!string.IsNullOrWhiteSpace(locationCode))
            {
                query = query.Where(o => o.Location.Code == locationCode);
            }

            var totalSales = await query.SumAsync(o => (decimal?)o.Total) ?? 0m;
            var orderCount = await query.CountAsync();
            var avgOrder = orderCount == 0 ? 0m : totalSales / orderCount;

            return Ok(new
            {
                date = today,
                locationCode = string.IsNullOrWhiteSpace(locationCode) ? "ALL" : locationCode,
                totalSales,
                orderCount,
                avgOrder
            });
        }
    }
}
