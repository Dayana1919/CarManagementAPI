using CarManagementAPI.Data;
using CarManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarManagementAPI.Controllers
{
    [ApiController]
    [Route("api/garages")]
    public class GarageController : ControllerBase
    {
        private readonly CarManagementAPIDbContext _context;

        public GarageController(CarManagementAPIDbContext context)
        {
            _context = context;
        }

        // GET: api/garages
        [HttpGet]
        public async Task<IActionResult> GetAllGarages([FromQuery] string? city)
        {
            var query = _context.Garages.AsQueryable();

            if (!string.IsNullOrEmpty(city))
            {
                query = query.Where(g => g.City == city);
            }

            var garages = await query.ToListAsync();
            return Ok(garages);
        }

        // GET: api/garages/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGarageById(int id)
        {
            var garage = await _context.Garages.FindAsync(id);

            if (garage == null)
            {
                return NotFound(new { message = "Garage not found" });
            }

            return Ok(garage);
        }

        // POST: api/garages
        [HttpPost]
        public async Task<IActionResult> CreateGarage([FromBody] Garage garage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Garages.Add(garage);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGarageById), new { id = garage.Id }, garage);
        }

        // PUT: api/garages/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGarage(int id, [FromBody] Garage updatedGarage)
        {
            if (id != updatedGarage.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            var existingGarage = await _context.Garages.FindAsync(id);

            if (existingGarage == null)
            {
                return NotFound(new { message = "Garage not found" });
            }

            existingGarage.Name = updatedGarage.Name;
            existingGarage.Location = updatedGarage.Location;
            existingGarage.City = updatedGarage.City;
            existingGarage.Capacity = updatedGarage.Capacity;

            await _context.SaveChangesAsync();

            return Ok(existingGarage);
        }

        // DELETE: api/garages/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGarage(int id)
        {
            var garage = await _context.Garages.FindAsync(id);

            if (garage == null)
            {
                return NotFound(new { message = "Garage not found" });
            }

            _context.Garages.Remove(garage);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Garage deleted successfully" });
        }
    }
}
