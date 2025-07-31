using DirectoryService.Domain;
using DirectoryService.Domain.PositionValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configuration;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("positions", "department");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(x => x.Id)
            .IsRequired()
            .HasColumnName("id")
            .HasConversion(
                value => value.Value,
                value => PositionId.Create(value).Value);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(PositionName.NAME_MAX_LENGHT)
            .HasColumnName("name")
            .HasConversion(
                value => value.Value,
                value => PositionName.Create(value).Value);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(Description.DESCRIPTION_MAX_LENGHT)
            .HasColumnName("description")
            .HasConversion(
                value => value == null ? null : value.Value,
                value => value == null ? null : Description.Create(value).Value);

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasColumnName("is_active");

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(x => x.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");
    }
}