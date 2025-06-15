using Microsoft.OpenApi.Models;
using Prometheus;
using Application.Extensions;
using Infrastructure.Extensions;
using Presentation.Middleware;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Zoo Management API", 
        Version = "v1",
        Description = "API для управления зоопарком"
    });
});

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

builder.Services.AddMemoryCache();

builder.Services.AddMetricServer(options => { });

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddScoped<Application.Services.ZooStatisticsService>();
builder.Services.AddScoped<Application.Services.AnimalTransferService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Zoo API v1"));

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseMetricServer();
app.UseHttpMetrics();

app.MapControllers();

app.Run(); 