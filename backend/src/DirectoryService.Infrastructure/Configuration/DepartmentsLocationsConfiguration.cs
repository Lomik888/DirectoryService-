using DirectoryService.Domain;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configuration;

public class DepartmentsLocationsConfiguration : IEntityTypeConfiguration<DepartmentsLocations>
{
    public void Configure(EntityTypeBuilder<DepartmentsLocations> builder)
    {
        builder.ToTable("departments_locations", "department");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.DepartmentId, x.LocationId }).IsUnique();

        builder.Property(x => x.Id)
            .IsRequired()
            .HasColumnName("id");

        builder.Property(x => x.DepartmentId)
            .IsRequired()
            .HasColumnName("department_id");

        builder.Property(x => x.LocationId)
            .IsRequired()
            .HasColumnName("location_id");

        builder.HasOne<Department>()
            .WithMany(x => x.DepartmentsLocations)
            .HasForeignKey(x => x.DepartmentId);

        builder.HasOne<Location>()
            .WithMany(x => x.DepartmentsLocations)
            .HasForeignKey(x => x.LocationId);
    }
}