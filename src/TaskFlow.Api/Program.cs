using Scalar.AspNetCore;
using TaskFlow.Api.Infrastructure;
using TaskFlow.Projects.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Aspire : OpenTelemetry, health checks, service discovery, résilience HTTP.
builder.AddServiceDefaults();

// Erreurs au format standard ProblemDetails (RFC 9457).
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddOpenApi();

// Modules métier (un appel par module).
builder.AddProjectsModule();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // Interface de test de l'API : /scalar
}

app.UseHttpsRedirection();

app.MapDefaultEndpoints();
app.MapInfoEndpoint();
app.MapProjectsModule();

app.Run();
