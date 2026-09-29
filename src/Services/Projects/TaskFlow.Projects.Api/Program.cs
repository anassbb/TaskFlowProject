using Scalar.AspNetCore;
using TaskFlow.Api.Common;
using TaskFlow.Projects.Api.Infrastructure;
using TaskFlow.Projects.Contracts;
using TaskFlow.Projects.Infrastructure;
using Wolverine;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Aspire : OpenTelemetry, health checks, service discovery, résilience HTTP.
builder.AddServiceDefaults();

// Client RabbitMQ Aspire : health check "RabbitMQ" visible dans /health.
builder.AddRabbitMQClient("rabbitmq");

// Wolverine : bus de messages. Ce service PUBLIE, il n'écoute aucune queue pour l'instant.
builder.Host.UseWolverine(opts =>
{
    var rabbitMq = builder.Configuration.GetConnectionString("rabbitmq")
        ?? throw new InvalidOperationException("Chaîne de connexion 'rabbitmq' absente : lancer via l'AppHost Aspire.");

    opts.UseRabbitMq(new Uri(rabbitMq))
        // Crée les exchanges/queues déclarés s'ils n'existent pas encore.
        .AutoProvision()
        // Fanout : chaque service abonné reçoit sa propre copie de l'événement.
        .DeclareExchange(ProjectsExchanges.ProjectCreated, exchange => exchange.ExchangeType = ExchangeType.Fanout);

    opts.PublishMessage<ProjectCreated>().ToRabbitExchange(ProjectsExchanges.ProjectCreated);
});

// Erreurs au format standard ProblemDetails (RFC 9457).
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddOpenApi();

builder.AddProjectsModule();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.Services.MigrateProjectsDatabaseAsync();
}

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

await app.RunAsync();
