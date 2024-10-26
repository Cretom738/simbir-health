using Application.Dtos;

namespace Application.Services
{
    public interface IAuthenticationService
    {
        Task<AuthDto> SignUpAsync(SignUpDto dto);

        Task<AuthDto> SignInAsync(SignInDto dto);

        Task SignOutAsync();

        Task<ValidationResultDto> ValidateAsync(ValidateDto dto);

        Task<AuthDto> RefreshAsync(RefreshDto dto);
    }
}
