using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pos.Api.Data;
using Pos.Api.Models;

namespace Pos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly PosDbContext _db;

        public LocationsController(PosDbContext db)
        {
            _db = db;
        }

        // GET /api/locations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetAll()
        {
            var locations = await _db.Locations
                .OrderBy(l => l.Name)
                .ToListAsync();

            return Ok(locations);
        }
    }
}

