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

    private Location(
        LocationName name,
        Timezone timezone,
        DateTime createdAt)
    {
        Id = LocationId.Create();
        Name = name;
        Timezone = timezone;
        IsActive = true;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public static Result<Location, Error.Error> Create(
        LocationName name,
        Timezone timezone,
        DateTime createdAt)
    {
        if (createdAt.Kind is not DateTimeKind.Utc)
        {
            var error = Error.Error.Create($"{nameof(createdAt)} must be UTC.");
            return error;
        }

        return new Location(name, timezone, createdAt);
    }

    public UnitResult<Error.Error> AddAddress(Address address)
    {
        if (Addresses.Contains(address))
        {
            var error = Error.Error.Create($"{nameof(address)} is already added.");
            return error;
        }

        _addresses.Add(address);
        return UnitResult.Success<Error.Error>();
    }
}