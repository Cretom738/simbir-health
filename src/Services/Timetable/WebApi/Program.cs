using System.Text.Json.Serialization;
using WebApi.Middleware;
using WebApi.Extensions;
using Application.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfigurations();

builder.AddDbContext();

builder.AddRabbitmq();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services
    .AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddHttpClient();
builder.ConfigureAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient(s => s.GetRequiredService<IHttpContextAccessor>()!.HttpContext!.User);
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddExceptionHandlingMiddleware();

builder.Services.AddEndpointsApiExplorer();
builder.ConfigureSwaggerAuthentication();

var app = builder.Build();

await app.MigrateDbAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandlingMiddleware();

app.MapControllers();

app.Run();