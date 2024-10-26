﻿using Domain.Events;
using Infrastructure.Data;
using Infrastructure.Messaging.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApi.Authentication;
using WebApi.Configurations;

namespace WebApi.Extensions
{
    public static class WebApplicationExtension
    {
        public static void AddConfigurations(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<TokenValidationConfiguration>(
                builder.Configuration.GetRequiredSection("TokenValidationConfiguration"));
        }

        public static void AddDbContext(this WebApplicationBuilder builder)
        {
            string connectionString = builder.Configuration.GetConnectionString("DatabaseConnection")!;

            builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseNpgsql(connectionString));
        }

        public static void AddRabbitmq(this WebApplicationBuilder builder)
        {
            string host = builder.Configuration["RabbitmqConfiguration:Host"]!;

            string username = builder.Configuration["RabbitmqConfiguration:Username"]!;

            string password = builder.Configuration["RabbitmqConfiguration:Password"]!;

            builder.Services.AddMassTransit(c =>
            {
                c.AddConsumer<CheckHospitalExistanceConsumer>();

                c.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(host, c =>
                    {
                        c.Username(username);

                        c.Password(password);
                    });

                    cfg.ReceiveEndpoint("hospital-existance", c =>
                    {
                        c.ConfigureConsumer<CheckHospitalExistanceConsumer>(context);
                    });
                });
            });
        }

        public static void ConfigureAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, TokenValidationAuthenticationHandler>("Bearer", null);
        }

        public static void ConfigureSwaggerAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Hospital Web API",
                    Version = "v1"
                });

                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                o.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        public static async Task MigrateDbAsync(this WebApplication app)
        {
            using IServiceScope serviceScope = app.Services.CreateScope();

            using ApplicationDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            bool canConnect;

            do
            {
                canConnect = await dbContext.Database.CanConnectAsync();

                if (canConnect)
                {
                    await dbContext.Database.MigrateAsync();
                }
            }
            while (!canConnect);
        }
    }
}