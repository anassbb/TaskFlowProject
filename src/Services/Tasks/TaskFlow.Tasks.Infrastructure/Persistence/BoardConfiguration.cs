using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Tasks.Domain;

namespace TaskFlow.Tasks.Infrastructure.Persistence;

internal sealed class BoardConfiguration : IEntityTypeConfiguration<Board>
{
    public void Configure(EntityTypeBuilder<Board> builder)
    {
        builder.ToTable("boards");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasConversion(id => id.Value, value => new BoardId(value))
            .ValueGeneratedNever();

        // Un seul board par projet : l'index unique est la seconde barrière de l'idempotence
        // (si deux messages identiques étaient traités en parallèle).
        builder.HasIndex(b => b.ProjectId).IsUnique();

        builder.Property(b => b.ProjectName).HasMaxLength(100).IsRequired();
        builder.Property(b => b.CreatedAt).IsRequired();

        // Liste de colonnes stockée en JSON (primitive collection EF Core), via le champ privé _columns.
        builder.PrimitiveCollection(b => b.Columns)
            .HasField("_columns")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(b => b.DomainEvents);
    }
}
