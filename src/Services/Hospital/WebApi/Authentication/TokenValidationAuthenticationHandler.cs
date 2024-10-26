using Application.Dtos;
using Domain.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using WebApi.Configurations;

namespace WebApi.Authentication
{
    public class TokenValidationAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly TokenValidationConfiguration _tokenValidationConfiguration;

        public TokenValidationAuthenticationHandler(
            IHttpClientFactory httpClientFactory,
            IOptions<TokenValidationConfiguration> tokenValidationConfiguration,
            IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder) 
            : base(options, logger, encoder)
        {
            _httpClientFactory = httpClientFactory;

            _tokenValidationConfiguration = tokenValidationConfiguration.Value;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string? authorizationHeaderValue = Request.Headers.Authorization;
            
            if (authorizationHeaderValue == null)
            {
                return AuthenticateResult.Fail("Authorization header is absent");
            }

            string[] bearer = authorizationHeaderValue.Split(' ');

            if (bearer.Length < 2)
            {
                return AuthenticateResult.Fail("Wrong authorization header format");
            }

            var builder = new UriBuilder(_tokenValidationConfiguration.ValidationUrl);

            var query = HttpUtility.ParseQueryString("");

            query["accessToken"] = bearer[1];

            builder.Query = query.ToString();

            string validationUrl = builder.ToString();

            HttpClient client = _httpClientFactory.CreateClient();

            using HttpResponseMessage response = await client.GetAsync(validationUrl);

            if (!response.IsSuccessStatusCode)
            {
                return AuthenticateResult.Fail("Access token expired");
            }

            ValidationResultDto validationResult = (await response.Content
                .ReadFromJsonAsync<ValidationResultDto>(new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter() },
                    PropertyNameCaseInsensitive = true
                }))!;

            IList<Claim> claims = [
                new Claim(ClaimTypes.NameIdentifier, validationResult.AccountId.ToString())
            ];

            foreach (Role role in validationResult.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            ClaimsIdentity identity = new ClaimsIdentity(claims, "Bearer");

            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            return AuthenticateResult.Success(new AuthenticationTicket(principal, "Bearer"));
        }
    }
}
