var builder = DistributedApplication.CreateBuilder(args);

// Base Postgres dans un conteneur (Podman). Le volume garde les données entre deux lancements.
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume("taskflow-postgres-data")
    .WithLifetime(ContainerLifetime.Persistent);

var database = postgres.AddDatabase("taskflowdb");

// API .NET : reçoit la chaîne de connexion de "taskflowdb" automatiquement.
var api = builder.AddProject<Projects.TaskFlow_Api>("api")
    .WithReference(database)
    .WaitFor(database)
    .WithHttpHealthCheck("/health");

// Front Angular : lancé avec "npm run start", l'URL de l'API lui est injectée en variable d'environnement.
builder.AddJavaScriptApp("web", "../../web/taskflow-ui", "start")
    .WithReference(api)
    .WaitFor(api)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints();

builder.Build().Run();
