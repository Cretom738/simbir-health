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
            services.AddScoped<IHospitalsService, HospitalsService>();

            services.AddScoped<IPublisher, Publisher>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IHospitalsRepository, HospitalsRepository>();
        }
    }
}
