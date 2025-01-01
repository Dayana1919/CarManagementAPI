using CarManagementAPI.Contracts;
using CarManagementAPI.DTOs.Car;
using CarManagementAPI.DTOs.Maintanace;
using CarManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CarManagementAPI.Controllers
{
    [Route("/cars")]
    [ApiController]
    public class CarController: ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ResponseCarDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery] string? make, [FromQuery] int? fromYear, [FromQuery] int? toYear, [FromQuery] int? garageId)
        {
            var cars = await _carService.GetFilteredAsync(make, fromYear, toYear, garageId);
            return Ok(cars);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseCarDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var car = await _carService.GetByIdAsync(id);
                return Ok(car);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCarDto car)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdId = await _carService.CreateAsync(car);
            return CreatedAtAction(nameof(GetById), new { id = createdId }, $"Car created with ID {createdId}");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseCarDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCarDto carDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _carService.UpdateAsync(id, carDto);
                var updatedCar = await _carService.GetByIdAsync(id);
                return Ok(updatedCar);
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
                var car = await _carService.GetByIdAsync(id);
                await _carService.DeleteAsync(new Car { Id = car.Id });
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
