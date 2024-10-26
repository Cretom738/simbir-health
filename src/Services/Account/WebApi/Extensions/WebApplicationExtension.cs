using Application.Configurations;
using Domain.Enums;
using Domain.Events;
using Infrastructure.Data;
using Infrastructure.Messaging.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Text;

namespace WebApi.Extensions
{
    public static class WebApplicationExtension
    {
        public static void AddConfigurations(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<JwtConfiguration>(
                builder.Configuration.GetRequiredSection("JwtConfiguration"));

            builder.Services.Configure<SessionsConfiguration>(
                builder.Configuration.GetRequiredSection("SessionsConfiguration"));
        }

        public static void AddDbContext(this WebApplicationBuilder builder)
        {
            string connectionString = builder.Configuration.GetConnectionString("DatabaseConnection")!;

            NpgsqlDataSourceBuilder dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

            dataSourceBuilder.MapEnum<Role>("role");

            NpgsqlDataSource dataSource = dataSourceBuilder.Build();

            builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseNpgsql(dataSource));
        }

        public static void AddRedis(this WebApplicationBuilder builder)
        {
            string connectionString = builder.Configuration.GetConnectionString("RedisConnection")!;

            builder.Services.AddStackExchangeRedisCache(o =>
            {
                o.Configuration = connectionString;
                o.InstanceName = "Account";
            });
        }

        public static void AddRabbitmq(this WebApplicationBuilder builder)
        {
            string host = builder.Configuration["RabbitmqConfiguration:Host"]!;

            string username = builder.Configuration["RabbitmqConfiguration:Username"]!;

            string password = builder.Configuration["RabbitmqConfiguration:Password"]!;

            builder.Services.AddMassTransit(c =>
            {
                c.AddConsumer<CheckAccountExistanceConsumer>();

                c.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(host, c =>
                    {
                        c.Username(username);

                        c.Password(password);

                        cfg.ReceiveEndpoint("account-existance", c =>
                        {
                            c.ConfigureConsumer<CheckAccountExistanceConsumer>(context);
                        });
                    });
                });
            });
        }

        public static void ConfigureAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = builder.Configuration["JwtConfiguration:Issuer"],
                    ValidAudience = builder.Configuration["JwtConfiguration:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JwtConfiguration:SigningKey"]!))
                });
        }

        public static void ConfigureSwaggerAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Account Web API", 
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
