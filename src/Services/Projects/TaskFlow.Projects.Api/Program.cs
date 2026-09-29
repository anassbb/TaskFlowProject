using Scalar.AspNetCore;
using TaskFlow.Projects.Api.Infrastructure;
using TaskFlow.Projects.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Aspire : OpenTelemetry, health checks, service discovery, résilience HTTP.
builder.AddServiceDefaults();

// Connexion RabbitMQ injectée par l'AppHost : ajoute health check et traces.
// (À l'étape 2, Wolverine utilisera cette même connexion pour publier les événements.)
builder.AddRabbitMQClient("rabbitmq");

// Erreurs au format standard ProblemDetails (RFC 9457).
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddOpenApi();

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
