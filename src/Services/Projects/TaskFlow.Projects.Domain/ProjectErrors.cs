using TaskFlow.SharedKernel;

namespace TaskFlow.Projects.Domain;

/// <summary>Erreurs métier du projet, avec un code stable que le front peut interpréter.</summary>
public static class ProjectErrors
{
    public static readonly Error NameRequired =
        Error.Validation("Project.NameRequired", "Le nom du projet est obligatoire.");

    public static readonly Error NameTooLong =
        Error.Validation("Project.NameTooLong", $"Le nom du projet ne peut pas dépasser {Project.NameMaxLength} caractères.");
}
