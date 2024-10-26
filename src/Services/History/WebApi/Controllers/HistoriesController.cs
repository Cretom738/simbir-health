using Application.Dtos;
using Application.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers
{
    [Route("api/History")]
    [ApiController]
    public class HistoriesController : ControllerBase
    {
        private readonly IHistoriesService _historiesService;

        public HistoriesController(IHistoriesService historiesService)
        {
            _historiesService = historiesService;
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.Manager)},{nameof(Role.Doctor)}")]
        [ProducesResponseType(typeof(HistoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HistoryDto>> CreateHistoryAsync([FromBody] CreateHistoryDto dto)
        {
            HistoryDto resultDto = await _historiesService.CreateHistoryAsync(dto);

            return Created(GetLocationPath(resultDto.Id), resultDto);
        }

        [HttpGet("Account/{accountId:long}")]
        [Authorize(Roles = $"{nameof(Role.Doctor)},{nameof(Role.User)}")]
        [ProducesResponseType(typeof(IEnumerable<HistoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<HistoryDto>>> GetHistoriesListByAccountIdAsync(
            [Range(1, long.MaxValue)] long accountId)
        {
            return Ok(await _historiesService.GetHistoriesListByAccountIdAsync(accountId));
        }

        [HttpGet("{id:long}")]
        [Authorize(Roles = $"{nameof(Role.Doctor)},{nameof(Role.User)}")]
        [ProducesResponseType(typeof(HistoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HistoryDto>> GetHistoryByIdAsync([Range(1, long.MaxValue)] long id)
        {
            return Ok(await _historiesService.GetHistoryByIdAsync(id));
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.Manager)},{nameof(Role.Doctor)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateHistoryByIdAsync([Range(1, long.MaxValue)] long id, [FromBody] UpdateHistoryDto dto)
        {
            await _historiesService.UpdateHistoryByIdAsync(id, dto);

            return NoContent();
        }

        private string GetLocationPath(long id)
        {
            return $"/api/History/{id}";
        }
    }
}
