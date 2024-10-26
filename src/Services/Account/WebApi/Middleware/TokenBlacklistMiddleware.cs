using Application.Classes.Constants;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;

namespace WebApi.Middleware
{
    public class TokenBlacklistMiddleware : IMiddleware
    {
        private readonly IDistributedCache _cache;

        public TokenBlacklistMiddleware(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!IsAnonymousAllowed(context))
            {
                long tokenPairId = long.Parse(context.User.FindFirstValue(CustomClaimTypes.TokenPairId)!);

                if (await IsTokenBlacklistedAsync(tokenPairId))
                {
                    throw new UnauthorizedException("auth.access_token_expired");
                }
            }

            await next(context);
        }

        private bool IsAnonymousAllowed(HttpContext context)
        {
            return context.GetEndpoint()?.Metadata?.GetMetadata<IAllowAnonymous>() != null;
        }

        private async Task<bool> IsTokenBlacklistedAsync(long tokenPairId)
        {
            return await _cache.GetStringAsync(GetBlacklistTokenKey(tokenPairId)) != null;
        }

        private string GetBlacklistTokenKey(long tokenPairId)
        {
            return $"token-pair-id-{tokenPairId}";
        }
    }

    public static class TokenBlacklistMiddlewareExtensions
    {
        public static IServiceCollection AddTokenBlacklistMiddleware(this IServiceCollection services)
        {
            return services.AddSingleton<TokenBlacklistMiddleware>();
        }

        public static IApplicationBuilder UseTokenBlacklistMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenBlacklistMiddleware>();
        }
    }
}
