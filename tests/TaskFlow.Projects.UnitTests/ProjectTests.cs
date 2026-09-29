using Shouldly;
using TaskFlow.Projects.Domain;

namespace TaskFlow.Projects.UnitTests;

public class ProjectTests
{
    private static readonly DateTimeOffset Now = new(2026, 9, 29, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Create_avec_un_nom_valide_cree_le_projet_et_leve_ProjectCreatedDomainEvent()
    {
        var result = Project.Create("  Refonte du site  ", Now);

        result.IsSuccess.ShouldBeTrue();
        var project = result.Value;
        project.Name.ShouldBe("Refonte du site"); // espaces retirés
        project.CreatedAt.ShouldBe(Now);
        project.Id.Value.ShouldNotBe(Guid.Empty);

        var domainEvent = project.DomainEvents.ShouldHaveSingleItem().ShouldBeOfType<ProjectCreatedDomainEvent>();
        domainEvent.ProjectId.ShouldBe(project.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_sans_nom_echoue_avec_NameRequired(string name)
    {
        var result = Project.Create(name, Now);

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(ProjectErrors.NameRequired);
    }

    [Fact]
    public void Create_avec_un_nom_trop_long_echoue_avec_NameTooLong()
    {
        var result = Project.Create(new string('x', Project.NameMaxLength + 1), Now);

        result.Error.ShouldBe(ProjectErrors.NameTooLong);
    }
}
