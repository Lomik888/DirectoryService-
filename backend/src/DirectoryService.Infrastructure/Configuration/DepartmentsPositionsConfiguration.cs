using DirectoryService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configuration;

public class DepartmentsPositionsConfiguration : IEntityTypeConfiguration<DepartmentsPositions>
{
    public void Configure(EntityTypeBuilder<DepartmentsPositions> builder)
    {
        builder.ToTable("departments_position", "department");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.DepartmentId, x.PositionId }).IsUnique();

        builder.Property(x => x.Id)
            .IsRequired()
            .HasColumnName("id");

        builder.Property(x => x.DepartmentId)
            .IsRequired()
            .HasColumnName("department_id");

        builder.Property(x => x.PositionId)
            .IsRequired()
            .HasColumnName("position_id");

        builder.HasOne<Department>()
            .WithMany(x => x.DepartmentsPositions)
            .HasForeignKey(x => x.DepartmentId);

        builder.HasOne<Position>()
            .WithMany(x => x.DepartmentsPositions)
            .HasForeignKey(x => x.PositionId);
    }
}