using CarManagementAPI.Contracts;
using CarManagementAPI.Data;
using CarManagementAPI.DTOs.Garage;
using CarManagementAPI.DTOs.Reports;
using CarManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CarManagementAPI.Controllers
{
    [ApiController]
    [Route("/garages")]
    public class GarageController : ControllerBase
    {
        
        private readonly IGarageService _garageService;

        public GarageController(IGarageService garageService)
        {
            _garageService = garageService;
        }

        [HttpGet("dailyAvailabilityReport")]
        [ProducesResponseType(typeof(IEnumerable<GarageDailyAvailabilityReportDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDailyAvailabilityReport([FromQuery] int garageId, [FromQuery] string startDate, [FromQuery] string endDate)
        {
            try
            {
                var report = await _garageService.GenerateDailyAvailabilityReportAsync(garageId, startDate, endDate);
                return Ok(report);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ResponseGarageDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery] string? city)
        {
            var garages = await _garageService.GetFilteredAsync(city);
            return Ok(garages);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseGarageDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var garage = await _garageService.GetByIdAsync(id);
                return Ok(garage);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] Garage garage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdId = await _garageService.CreateAsync(garage);
            return CreatedAtAction(nameof(GetById), new { id = createdId }, $"Garage created with ID {createdId}");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseGarageDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateGarageDto garageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _garageService.UpdateAsync(id, garageDto);
                var updatedGarage = await _garageService.GetByIdAsync(id);
                return Ok(updatedGarage);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var garage = await _garageService.GetByIdAsync(id);
                await _garageService.DeleteAsync(new Garage { Id = garage.Id });
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
