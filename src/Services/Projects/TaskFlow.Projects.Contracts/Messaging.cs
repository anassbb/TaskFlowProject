namespace TaskFlow.Projects.Contracts;

/// <summary>
/// Noms des exchanges RabbitMQ sur lesquels le service Projects publie.
/// Un exchange "fanout" par type d'événement : chaque service abonné y lie SA propre queue.
/// </summary>
public static class ProjectsExchanges
{
    public const string ProjectCreated = "projects.project-created";
}
