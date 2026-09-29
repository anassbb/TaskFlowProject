using TaskFlow.SharedKernel;

namespace TaskFlow.Projects.Domain;

/// <summary>
/// Événement de DOMAINE : interne au service, porte des types du domaine (ProjectId).
/// À ne pas confondre avec l'événement d'INTÉGRATION <c>TaskFlow.Projects.Contracts.ProjectCreated</c>,
/// public et sérialisé sur RabbitMQ.
/// </summary>
public sealed record ProjectCreatedDomainEvent(ProjectId ProjectId, string Name) : DomainEvent;
