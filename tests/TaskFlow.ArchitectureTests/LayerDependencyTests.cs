using System.Reflection;
using NetArchTest.Rules;
using Shouldly;

namespace TaskFlow.ArchitectureTests;

/// <summary>
/// Vérifie la règle de dépendance de la Clean Architecture :
/// Domain ← Application ← Infrastructure ← Api. Un test rouge = une couche qui en référence une autre à tort.
/// </summary>
public class LayerDependencyTests
{
    private static readonly Assembly SharedKernel = typeof(SharedKernel.Result).Assembly;
    private static readonly Assembly ProjectsDomain = Assembly.Load("TaskFlow.Projects.Domain");
    private static readonly Assembly ProjectsApplication = typeof(Projects.Application.DependencyInjection).Assembly;
    private static readonly Assembly ProjectsInfrastructure = typeof(Projects.Infrastructure.ProjectsModule).Assembly;

    [Fact]
    public void SharedKernel_ne_depend_de_rien_d_externe()
    {
        var result = Types.InAssembly(SharedKernel)
            .ShouldNot().HaveDependencyOnAny("Microsoft.EntityFrameworkCore", "Microsoft.AspNetCore", "TaskFlow.Projects")
            .GetResult();

        result.IsSuccessful.ShouldBeTrue(Describe(result));
    }

    [Fact]
    public void Domain_ne_depend_ni_d_EF_Core_ni_d_ASP_NET_ni_des_couches_superieures()
    {
        var result = Types.InAssembly(ProjectsDomain)
            .ShouldNot().HaveDependencyOnAny(
                "Microsoft.EntityFrameworkCore",
                "Microsoft.AspNetCore",
                "TaskFlow.Projects.Application",
                "TaskFlow.Projects.Infrastructure",
                "TaskFlow.Api")
            .GetResult();

        result.IsSuccessful.ShouldBeTrue(Describe(result));
    }

    [Fact]
    public void Application_ne_depend_pas_de_l_Infrastructure_ni_d_EF_Core()
    {
        var result = Types.InAssembly(ProjectsApplication)
            .ShouldNot().HaveDependencyOnAny(
                "Microsoft.EntityFrameworkCore",
                "TaskFlow.Projects.Infrastructure",
                "TaskFlow.Api")
            .GetResult();

        result.IsSuccessful.ShouldBeTrue(Describe(result));
    }

    [Fact]
    public void Infrastructure_ne_depend_pas_de_l_Api()
    {
        var result = Types.InAssembly(ProjectsInfrastructure)
            .ShouldNot().HaveDependencyOn("TaskFlow.Api")
            .GetResult();

        result.IsSuccessful.ShouldBeTrue(Describe(result));
    }

    [Fact]
    public void Les_entites_du_domaine_sont_sealed()
    {
        var result = Types.InAssembly(ProjectsDomain)
            .That().Inherit(typeof(SharedKernel.Entity<>))
            .Should().BeSealed()
            .GetResult();

        result.IsSuccessful.ShouldBeTrue(Describe(result));
    }

    private static string Describe(NetArchTest.Rules.TestResult result) =>
        "Types en infraction : " + string.Join(", ", result.FailingTypeNames ?? []);
}
