using CarManagementAPI.Contracts;
using CarManagementAPI.Data;
using CarManagementAPI.DTOs.Maintanace;
using CarManagementAPI.DTOs.Reports;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CarManagementAPI.Controllers
{
    [Route("/maintenance")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private readonly IMaintenanceService _maintenanceService;

        public MaintenanceController(IMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService;
        }

        [HttpGet("monthlyRequestsReport")]
        [ProducesResponseType(typeof(IEnumerable<MonthlyRequestsReportDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMonthlyRequestsReport([FromQuery] int? garageId, [FromQuery] string startMonth, [FromQuery] string endMonth)
        {
            try
            {
                var report = await _maintenanceService.GenerateMonthlyReportAsync(garageId ?? 0, startMonth, endMonth);
                return Ok(report);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ResponseMaintenanceDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery] int? carId, [FromQuery] int? garageId, [FromQuery] string? startDate, [FromQuery] string? endDate)
        {
            var maintenances = await _maintenanceService.GetFilteredAsync(carId, garageId, startDate, endDate);
            return Ok(maintenances);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseMaintenanceDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var maintenance = await _maintenanceService.GetByIdAsync(id);
                return Ok(maintenance);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] Maintenance maintenance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdId = await _maintenanceService.CreateAsync(maintenance);
            return CreatedAtAction(nameof(GetById), new { id = createdId }, $"Maintenance created with ID {createdId}");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseMaintenanceDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMaintenanceDto maintenanceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _maintenanceService.UpdateAsync(id, maintenanceDto);
                var updatedMaintenance = await _maintenanceService.GetByIdAsync(id);
                return Ok(updatedMaintenance);
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
                var maintenance = await _maintenanceService.GetByIdAsync(id);
                await _maintenanceService.DeleteAsync(new Maintenance { Id = maintenance.Id });
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }

    }
