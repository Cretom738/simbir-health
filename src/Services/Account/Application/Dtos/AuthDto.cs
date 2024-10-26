namespace Application.Dtos
{
    public class AuthDto
    {
        public required string RefreshToken { get; set; }

        public required string AccessToken { get; set; }
    }
}
