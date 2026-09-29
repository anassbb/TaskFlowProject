namespace TaskFlow.Tasks.Api;

/// <summary>
/// Queues RabbitMQ propres au service Tasks. Convention : "{service consommateur}.{événement}".
/// </summary>
public static class TasksQueues
{
    public const string ProjectCreated = "tasks.project-created";
}
