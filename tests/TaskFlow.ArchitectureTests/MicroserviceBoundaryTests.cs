using System.Reflection;
using Shouldly;

namespace TaskFlow.ArchitectureTests;

/// <summary>
/// Frontières entre microservices : un service ne connaît des autres que leur projet *.Contracts.
/// On vérifie les références d'assemblies (ce que le compilateur a réellement lié).
/// </summary>
public class MicroserviceBoundaryTests
{
    private static readonly Assembly Gateway = Assembly.Load("TaskFlow.Gateway");
    private static readonly Assembly ProjectsContracts = Assembly.Load("TaskFlow.Projects.Contracts");

    private static IEnumerable<string> TaskFlowReferences(Assembly assembly) =>
        assembly.GetReferencedAssemblies()
            .Select(a => a.Name!)
            .Where(name => name.StartsWith("TaskFlow.", StringComparison.Ordinal));

    [Fact]
    public void La_Gateway_ne_reference_aucun_microservice()
    {
        // La Gateway route par HTTP : elle ne doit dépendre que des briques techniques communes.
        var references = TaskFlowReferences(Gateway).ToList();

        references.ShouldAllBe(name => name == "TaskFlow.ServiceDefaults",
            $"Références interdites : {string.Join(", ", references)}");
    }

    [Fact]
    public void Les_Contracts_ne_dependent_d_aucun_autre_projet_TaskFlow()
    {
        // Un contrat public doit pouvoir être référencé par n'importe quel service sans rien entraîner.
        TaskFlowReferences(ProjectsContracts).ShouldBeEmpty();
    }
}
