using Application.Dtos;
using Application.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers
{
    [Route("api/Accounts")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountsService;

        public AccountsController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        [HttpPost]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AccountDto>> CreateAccountAsync([FromBody] CreateAccountDto dto)
        {
            AccountDto resultDto = await _accountsService.CreateAccountAsync(dto);

            return Created(GetLocationPath(resultDto.Id), resultDto);
        }

        [HttpGet]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(IEnumerable<AccountDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccountsListAsync([FromQuery] FilterAccountsDto dto)
        {
            return Ok(await _accountsService.GetAccountsListAsync(dto));
        }

        [HttpGet("Me")]
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AccountDto>> GetCurrentAccountAsync()
        {
            return Ok(await _accountsService.GetCurrentAccountAsync());
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateAccountByIdAsync(
            [Range(1, long.MaxValue)] long id, [FromBody] UpdateAccountDto dto)
        {
            await _accountsService.UpdateAccountByIdAsync(id, dto);

            return NoContent();
        }

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> UpdateCurrentAccountAsync([FromBody] UpdateCurrentAccountDto dto)
        {
            await _accountsService.UpdateCurrentAccountAsync(dto);

            return NoContent();
        }

        [HttpDelete("{id:long}")]
        [Authorize(Roles = nameof(Role.Admin))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAccountByIdAsync([Range(1, long.MaxValue)] long id)
        {
            await _accountsService.DeleteAccountByIdAsync(id);

            return NoContent();
        }

        private string GetLocationPath(long id)
        {
            return $"/api/Account/{id}";
        }
    }
}
