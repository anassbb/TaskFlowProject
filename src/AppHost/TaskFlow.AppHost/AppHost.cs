var builder = DistributedApplication.CreateBuilder(args);

// ── Infrastructure partagée (conteneurs Podman, données conservées entre deux lancements) ──

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume("taskflow-postgres-data")
    .WithLifetime(ContainerLifetime.Persistent);

// Une base par service : aucun service ne lit la base d'un autre.
var projectsDb = postgres.AddDatabase("projectsdb");

// Broker de messages. Le plugin "management" ajoute l'interface web (files, débits, messages en attente).
var rabbitmq = builder.AddRabbitMQ("rabbitmq")
    .WithManagementPlugin()
    .WithDataVolume("taskflow-rabbitmq-data")
    .WithLifetime(ContainerLifetime.Persistent);

// ── Microservices ──

var projectsApi = builder.AddProject<Projects.TaskFlow_Projects_Api>("projects-api")
    .WithReference(projectsDb)
    .WithReference(rabbitmq)
    .WaitFor(projectsDb)
    .WaitFor(rabbitmq)
    .WithHttpHealthCheck("/health");

// ── Gateway : seul point d'entrée du front ──

var gateway = builder.AddProject<Projects.TaskFlow_Gateway>("gateway")
    .WithReference(projectsApi)
    .WaitFor(projectsApi)
    .WithHttpHealthCheck("/health");

// ── Front Angular : ne connaît que la Gateway ──

builder.AddJavaScriptApp("web", "../../../web/taskflow-ui", "start")
    .WithReference(gateway)
    .WaitFor(gateway)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints();

builder.Build().Run();
