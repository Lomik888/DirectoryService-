using System.Text.Json;
using DirectoryService.Domain;
using DirectoryService.Domain.LocationValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configuration;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations", "department");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(x => x.Id)
            .IsRequired()
            .HasColumnName("id")
            .HasConversion(
                value => value.Value,
                value => LocationId.Create(value).Value);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(LocationName.NAME_MAX_LENGHT)
            .HasColumnName("name")
            .HasConversion(
                value => value.Value,
                value => LocationName.Create(value).Value);

        builder.Property(x => x.Addresses)
            .HasConversion(
                value => JsonSerializer.Serialize(value, JsonSerializerOptions.Default),
                valueDb =>
                    JsonSerializer.Deserialize<IReadOnlyList<Address>>(valueDb, JsonSerializerOptions.Default)!,
                new ValueComparer<IReadOnlyList<Address>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()))
            .HasColumnName("addresses")
            .HasColumnType("jsonb");

        builder.Property(x => x.Timezone)
            .IsRequired()
            .HasColumnName("timezone")
            .HasConversion(
                value => value.Value,
                value => Timezone.Create(value).Value);

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