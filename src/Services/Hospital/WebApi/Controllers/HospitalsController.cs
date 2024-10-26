using Application.Dtos;
using Application.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers
{
    [Route("api/Hospitals")]
    [ApiController]
    [Authorize]
    public class HospitalsController : ControllerBase
    {
        private readonly IHospitalsService _hospitalsService;

        public HospitalsController(IHospitalsService hospitalsService)
        {
            _hospitalsService = hospitalsService;
        }

        [HttpPost]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(HospitalDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<HospitalDto>> CreateHospitalAsync([FromBody] CreateHospitalDto dto)
        {
            HospitalDto resultDto = await _hospitalsService.CreateHospitalAsync(dto);

            return Created(GetLocationPath(resultDto.Id), resultDto);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HospitalDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<HospitalDto>>> GetHospitalsListAsync([FromQuery] FilterHospitalsDto dto)
        {
            return Ok(await _hospitalsService.GetHospitalsListAsync(dto));
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(HospitalDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HospitalDto>> GetHospitalByIdAsync([Range(1, long.MaxValue)] long id)
        {
            return Ok(await _hospitalsService.GetHospitalByIdAsync(id));
        }

        [HttpGet("{id:long}/Rooms")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<string>>> GetHospitalRoomsListAsync([Range(1, long.MaxValue)] long id)
        {
            return Ok(await _hospitalsService.GetHospitalRoomsListAsync(id));
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateHospitalByIdAsync(
            [Range(1, long.MaxValue)] long id, [FromBody] UpdateHospitalDto dto)
        {
            await _hospitalsService.UpdateHospitalByIdAsync(id, dto);

            return NoContent();
        }

        [HttpDelete("{id:long}")]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteHospitalByIdAsync([Range(1, long.MaxValue)] long id)
        {
            await _hospitalsService.DeleteHospitalByIdAsync(id);

            return NoContent();
        }

        private string GetLocationPath(long id)
        {
            return $"/api/Hospitals/{id}";
        }
    }
}
