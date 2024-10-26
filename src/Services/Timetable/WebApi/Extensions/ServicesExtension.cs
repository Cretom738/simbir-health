using Application.Services;
using Domain;
using Domain.Interfaces;
using Domain.Repositories;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Messaging;

namespace WebApi.Extensions
{
    public static class ServicesExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITimetablesService, TimetablesService>();

            services.AddScoped<IAppointmentsService, AppointmentsService>();

            services.AddScoped<IRequester, Requester>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ITimetablesRepository, TimetablesRepository>();

            services.AddScoped<IAppointmentsRepository, AppointmentsRepository>();
        }
    }
}
