using System.Reflection;
using NetArchTest.Rules;
using Shouldly;

namespace TaskFlow.ArchitectureTests;

/// <summary>Même règle de dépendance que pour Projects, appliquée au service Tasks.</summary>
public class TasksLayerDependencyTests
{
    private static readonly Assembly TasksDomain = Assembly.Load("TaskFlow.Tasks.Domain");
    private static readonly Assembly TasksApplication = Assembly.Load("TaskFlow.Tasks.Application");

    [Fact]
    public void Domain_ne_depend_ni_d_EF_Core_ni_d_ASP_NET_ni_d_un_autre_service()
    {
        var result = Types.InAssembly(TasksDomain)
            .ShouldNot().HaveDependencyOnAny(
                "Microsoft.EntityFrameworkCore",
                "Microsoft.AspNetCore",
                "TaskFlow.Tasks.Application",
                "TaskFlow.Tasks.Infrastructure",
                "TaskFlow.Projects")
            .GetResult();

        result.IsSuccessful.ShouldBeTrue(LayerDependencyTests.Describe(result));
    }

    [Fact]
    public void Application_ne_depend_ni_d_EF_Core_ni_de_Wolverine()
    {
        // Les handlers d'événements restent de simples classes : Wolverine les découvre par convention.
        var result = Types.InAssembly(TasksApplication)
            .ShouldNot().HaveDependencyOnAny("Microsoft.EntityFrameworkCore", "Wolverine", "TaskFlow.Tasks.Infrastructure")
            .GetResult();

        result.IsSuccessful.ShouldBeTrue(LayerDependencyTests.Describe(result));
    }
}
