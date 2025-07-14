using CSharpFunctionalExtensions;
using DirectoryService.Domain.LocationValueObjects;

namespace DirectoryService.Domain;

public sealed class Location : Entity<LocationId>
{
    private List<Address> _addresses = [];
    private List<Department> _departments = [];

    public LocationName Name { get; private set; }

    public Timezone Timezone { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyList<Address> Addresses => _addresses;

    public IReadOnlyList<Department> Departments => _departments;

    public Location(
        LocationName name,
        Timezone timezone)
    {
        Id = LocationId.Create();
        Name = name;
        Timezone = timezone;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }
}