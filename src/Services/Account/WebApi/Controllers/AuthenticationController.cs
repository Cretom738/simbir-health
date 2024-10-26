using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        [ProducesResponseType(typeof(AuthDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AuthDto>> SignUpAsync([FromBody] SignUpDto dto)
        {
            return Created(GetLocationPath(), await _authenticationService.SignUpAsync(dto));
        }

        [AllowAnonymous]
        [HttpPost("SignIn")]
        [ProducesResponseType(typeof(AuthDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthDto>> SignInAsync([FromBody] SignInDto dto)
        {
            return Ok(await _authenticationService.SignInAsync(dto));
        }

        [HttpPut("SignOut")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> SignOutAsync()
        {
            await _authenticationService.SignOutAsync();

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("Validate")]
        [ProducesResponseType(typeof(ValidationResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ValidationResultDto>> ValidateAsync([FromQuery] ValidateDto dto)
        {
            return Ok(await _authenticationService.ValidateAsync(dto));
        }

        [AllowAnonymous]
        [HttpPost("Refresh")]
        [ProducesResponseType(typeof(AuthDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthDto>> RefreshAsync([FromBody] RefreshDto dto)
        {
            return Ok(await _authenticationService.RefreshAsync(dto));
        }

        private string GetLocationPath()
        {
            return $"/api/Account/Me";
        }
    }
}
