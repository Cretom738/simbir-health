using Application.Dtos;
using Application.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers
{
    [Route("api/Timetable")]
    [ApiController]
    public class TimetablesController : ControllerBase
    {
        private readonly ITimetablesService _timetablesService;

        private readonly IAppointmentsService _appointmentsService;

        public TimetablesController(
            ITimetablesService timetablesService,
            IAppointmentsService appointmentsService)
        {
            _timetablesService = timetablesService;

            _appointmentsService = appointmentsService;
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.Manager)}")]
        [ProducesResponseType(typeof(TimetableDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TimetableDto>> CreateTimetableAsync([FromBody] CreateTimetableDto dto)
        {
            TimetableDto resultDto = await _timetablesService.CreateTimetableAsync(dto);

            return Created(GetLocationPath(resultDto.Id), resultDto);
        }

        [HttpPost("{timetableId:long}/Appointments")]
        [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AppointmentDto>> CreateAppointmentAsync(
            [Range(1, long.MaxValue)] long timetableId, [FromBody] CreateAppointmentDto dto)
        {
            AppointmentDto resultDto = await _appointmentsService.CreateAppointmentAsync(timetableId, dto);

            return Created(GetLocationPath(resultDto.Id), resultDto);
        }

        [HttpGet("Doctor/{doctorId:long}")]
        [ProducesResponseType(typeof(IEnumerable<TimetableDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<TimetableDto>>> GetTimetablesListByDoctorIdAsync(
            [Range(1, long.MaxValue)] long doctorId, [FromQuery] FilterTimetablesDto dto)
        {
            return Ok(await _timetablesService.GetTimetablesListByDoctorIdAsync(doctorId, dto));
        }

        [HttpGet("Hospital/{hospitalId:long}")]
        [ProducesResponseType(typeof(IEnumerable<TimetableDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<TimetableDto>>> GetTimetablesListByHospitalIdAsync(
            [Range(1, long.MaxValue)] long hospitalId, [FromQuery] FilterTimetablesDto dto)
        {
            return Ok(await _timetablesService.GetTimetablesListByHospitalIdAsync(hospitalId, dto));
        }

        [HttpGet("Hospital/{hospitalId:long}/Room/{room}")]
        [Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.Manager)},{nameof(Role.Doctor)}")]
        [ProducesResponseType(typeof(IEnumerable<TimetableDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<TimetableDto>>> GetTimetablesListByHospitalRoomAsync(
            [Range(1, long.MaxValue)] long hospitalId,
            [Required(AllowEmptyStrings = false), MaxLength(200)] string room,
            [FromQuery] FilterTimetablesDto dto)
        {
            return Ok(await _timetablesService.GetTimetablesListByHospitalRoomAsync(hospitalId, room, dto));
        }

        [HttpGet("{timetableId:long}/Appointments")]
        [ProducesResponseType(typeof(IEnumerable<DateTime>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<DateTime>>> GetAvaliableAppointmentsListByAsync(
            [Range(1, long.MaxValue)] long timetableId)
        {
            return Ok(await _appointmentsService.GetAvaliableAppointmentsListByAsync(timetableId));
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.Manager)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateTimetableByIdAsync([Range(1, long.MaxValue)] long id, [FromBody] UpdateTimetableDto dto)
        {
            await _timetablesService.UpdateTimetableByIdAsync(id, dto);

            return NoContent();
        }

        [HttpDelete("Doctor/{doctorId:long}")]
        [Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.Manager)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> DeleteTimetablesByDoctorIdAsync([Range(1, long.MaxValue)] long doctorId)
        {
            await _timetablesService.DeleteTimetablesByDoctorIdAsync(doctorId);

            return NoContent();
        }

        [HttpDelete("Hospital/{hospitalId:long}")]
        [Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.Manager)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> DeleteTimetablesByHospitalIdAsync([Range(1, long.MaxValue)] long hospitalId)
        {
            await _timetablesService.DeleteTimetablesByHospitalIdAsync(hospitalId);

            return NoContent();
        }

        [HttpDelete("{id:long}")]
        [Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.Manager)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTimetableByIdAsync([Range(1, long.MaxValue)] long id)
        {
            await _timetablesService.DeleteTimetableByIdAsync(id);

            return NoContent();
        }

        private string GetLocationPath(long id)
        {
            return $"/api/Timetable/{id}";
        }
    }
}
