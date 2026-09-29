using JasperFx.CodeGeneration.Model;
using Scalar.AspNetCore;
using TaskFlow.Api.Common;
using TaskFlow.Projects.Contracts;
using TaskFlow.Tasks.Api;
using TaskFlow.Tasks.Application.IntegrationEvents;
using TaskFlow.Tasks.Infrastructure;
using Wolverine;
using Wolverine.RabbitMQ;
using Wolverine.RabbitMQ.Internal;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRabbitMQClient("rabbitmq");

// Wolverine : ce service ÉCOUTE les événements des autres services.
builder.Host.UseWolverine(opts =>
{
    var rabbitMq = builder.Configuration.GetConnectionString("rabbitmq")
        ?? throw new InvalidOperationException("Chaîne de connexion 'rabbitmq' absente : lancer via l'AppHost Aspire.");

    opts.UseRabbitMq(new Uri(rabbitMq))
        .AutoProvision()
        // Même exchange que celui déclaré par Projects (la déclaration est idempotente),
        // auquel on lie la queue PROPRE à Tasks : chaque service abonné a sa copie du message.
        .DeclareExchange(ProjectsExchanges.ProjectCreated, exchange =>
        {
            exchange.ExchangeType = ExchangeType.Fanout;
            exchange.BindQueue(TasksQueues.ProjectCreated);
        });

    // Quorum queue : durable et répliquée, le type recommandé depuis RabbitMQ 4.
    opts.ListenToRabbitQueue(TasksQueues.ProjectCreated, queue => queue.QueueType = QueueType.quorum);

    // Les handlers vivent dans la couche Application, pas dans l'API.
    opts.Discovery.IncludeAssembly(typeof(ProjectCreatedHandler).Assembly);

    // Le DbContext est enregistré par Aspire via un pool (fabrique opaque) : Wolverine doit alors le
    // demander au conteneur au lieu de l'instancier dans son code généré. Autorisé, avec un avertissement.
    // (Étape 3 : l'intégration EF Core de Wolverine, installée pour l'outbox, supprimera ce besoin.)
    opts.ServiceLocationPolicy = ServiceLocationPolicy.AllowedButWarn;
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddOpenApi();

builder.AddTasksModule();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.Services.MigrateTasksDatabaseAsync();
}

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapDefaultEndpoints();
app.MapInfoEndpoint();
app.MapTasksModule();

await app.RunAsync();
