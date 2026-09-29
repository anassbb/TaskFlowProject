using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// YARP : les routes et les clusters sont décrits dans appsettings.json (section "ReverseProxy").
// Les adresses du type "https+http://projects-api" sont résolues par le service discovery d'Aspire.
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapDefaultEndpoints();

// Endpoint propre à la Gateway : le front l'utilise pour afficher "API connectée".
app.MapGet("/api/info", (IHostEnvironment env) => TypedResults.Ok(new
{
    Name = "TaskFlow Gateway",
    Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0",
    Environment = env.EnvironmentName,
    ServerTime = DateTimeOffset.UtcNow,
}));

app.MapReverseProxy();

app.Run();
