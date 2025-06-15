using Microsoft.OpenApi.Models;
using Prometheus;
using Application.Extensions;
using Infrastructure.Extensions;
using Presentation.Middleware;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Zoo Management API", 
        Version = "v1",
        Description = "API для управления зоопарком"
    });
});

// Add logging
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

// Add memory cache
builder.Services.AddMemoryCache();

// Add metrics
builder.Services.AddMetricServer(options => { });

// Add application services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddScoped<Application.Services.ZooStatisticsService>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Zoo API v1"));

app.UseHttpsRedirection();
app.UseAuthorization();

// Add error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

// Add metrics endpoints
app.UseMetricServer();
app.UseHttpMetrics();

app.MapControllers();

app.Run(); 