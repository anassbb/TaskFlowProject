using Shouldly;
using TaskFlow.SharedKernel;

namespace TaskFlow.Projects.UnitTests;

/// <summary>Exemple de tests unitaires : ton domaine suivra le même modèle (Arrange / Act / Assert).</summary>
public class ResultTests
{
    private static readonly Error SampleError = Error.Validation("Project.NameRequired", "Le nom du projet est obligatoire.");

    [Fact]
    public void Success_porte_la_valeur()
    {
        Result<int> result = 42;

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(42);
        result.Error.ShouldBe(Error.None);
    }

    [Fact]
    public void Failure_porte_l_erreur_et_interdit_la_lecture_de_la_valeur()
    {
        Result<int> result = SampleError;

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(SampleError);
        Should.Throw<InvalidOperationException>(() => result.Value);
    }
}
