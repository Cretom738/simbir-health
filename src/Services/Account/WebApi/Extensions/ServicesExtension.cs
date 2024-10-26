using Application.Services;
using Domain.Repositories;
using Domain;
using Infrastructure.Data.Repositories;
using Infrastructure.Data;
using Domain.Interfaces;
using Infrastructure.Messaging;

namespace WebApi.Extensions
{
    public static class ServicesExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IArgonService, ArgonService>();

            services.AddScoped<IJwtService, JwtService>();

            services.AddScoped<ISessionsService, SessionsService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddScoped<IAccountsService, AccountsService>();

            services.AddScoped<IPublisher, Publisher>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAccountsRepository, AccountsRepository>();

            services.AddScoped<ISessionsRepository, SessionsRepository>();
        }
    }
}
