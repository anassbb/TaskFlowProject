using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Projects.Domain;

namespace TaskFlow.Projects.Infrastructure.Persistence;

/// <summary>Mapping EF Core de l'agrégat : le domaine reste libre de tout attribut de persistance.</summary>
internal sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("projects");

        builder.HasKey(p => p.Id);

        // Identifiant typé <-> uuid en base.
        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, value => new ProjectId(value))
            .ValueGeneratedNever();

        builder.Property(p => p.Name)
            .HasMaxLength(Project.NameMaxLength)
            .IsRequired();

        builder.Property(p => p.CreatedAt).IsRequired();

        // Les événements de domaine ne sont pas persistés.
        builder.Ignore(p => p.DomainEvents);
    }
}
