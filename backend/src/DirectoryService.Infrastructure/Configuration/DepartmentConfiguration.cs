using DirectoryService.Domain;
using DirectoryService.Domain.DepartmentValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Path = DirectoryService.Domain.DepartmentValueObjects.Path;

namespace DirectoryService.Infrastructure.Configuration;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments", "department");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .HasColumnName("id")
            .HasConversion(
                value => value.Value,
                value => DepartmentId.Create(value).Value);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(DepartmentName.NAME_MAX_LENGHT)
            .HasColumnName("name")
            .HasConversion(
                value => value.Value,
                value => DepartmentName.Create(value).Value);

        builder.Property(x => x.Identifier)
            .IsRequired()
            .HasMaxLength(Identifier.IDENTIFIER_MAX_LENGHT)
            .HasColumnName("identifier")
            .HasConversion(
                value => value.Value,
                value => Identifier.Create(value).Value);

        builder.Property(x => x.ParentId)
            .IsRequired(false)
            .HasConversion(
                value => value == null ? (Guid?)null : value.Value,
                value => value == null ? null : DepartmentId.Create((Guid)value).Value);

        builder.Property(x => x.Path)
            .IsRequired()
            .HasColumnName("path")
            .HasConversion(
                value => value.Value,
                value => Path.Create(value).Value);

        builder.Property(x => x.Depth)
            .IsRequired()
            .HasColumnName("depth");

        builder.Property(x => x.ChildrenCount)
            .IsRequired()
            .HasColumnName("children_count");

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasColumnName("is_active");

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(x => x.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.HasMany(x => x.ChildrenDepartments)
            .WithOne()
            .IsRequired(false)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Positions)
            .WithMany(x => x.Departments)
            .UsingEntity(
                "departments_position",
                r => r
                    .HasOne(typeof(Position))
                    .WithMany()
                    .HasForeignKey("position_id"),
                l => l
                    .HasOne(typeof(Department))
                    .WithMany()
                    .HasForeignKey("department_id"),
                j => j.HasKey("position_id", "department_id"));

        builder.HasMany(x => x.Locations)
            .WithMany(x => x.Departments)
            .UsingEntity(
                "departments_locations",
                r => r
                    .HasOne(typeof(Location))
                    .WithMany()
                    .HasForeignKey("location_id"),
                l => l
                    .HasOne(typeof(Department))
                    .WithMany()
                    .HasForeignKey("department_id"),
                j => j.HasKey("location_id", "department_id"));
    }
}