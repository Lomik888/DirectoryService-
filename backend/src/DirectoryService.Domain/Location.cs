using CSharpFunctionalExtensions;
using DirectoryService.Domain.Abstractions;
using DirectoryService.Domain.Err;
using DirectoryService.Domain.LocationValueObjects;

namespace DirectoryService.Domain;

public sealed class Location : Entity<LocationId>
{
    private List<Address> _addresses = [];
    private readonly List<DepartmentsLocations> _departmentsLocations = [];

    public LocationName Name { get; private set; }

    public Timezone Timezone { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyList<Address> Addresses => _addresses;

    public IReadOnlyList<DepartmentsLocations> DepartmentsLocations => _departmentsLocations;

    private Location()
    {
    }

    private Location(
        LocationName name,
        Timezone timezone,
        Address address,
        DateTime createdAt)
    {
        Id = LocationId.Create();
        Name = name;
        Timezone = timezone;
        IsActive = true;
        _addresses.Add(address);
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public static Result<Location, Error> Create(
        LocationName name,
        Timezone timezone,
        Address address,
        IClock clock)
    {
        var createdAt = clock.UtcNow();

        if (createdAt.Kind is not DateTimeKind.Utc)
        {
            var error = Error.Create(
                $"{nameof(createdAt)} must be UTC.",
                "invalid.parameter",
                ErrorTypes.VALIDATION);
            return error;
        }

        return new Location(name, timezone, address, createdAt);
    }

    public UnitResult<Error> AddAddress(Address address)
    {
        if (Addresses.Contains(address))
        {
            var error = Error.Create(
                $"{nameof(address)} is already added.",
                "invalid.parameter",
                ErrorTypes.VALIDATION);
            return error;
        }

        _addresses.Add(address);
        return UnitResult.Success<Error>();
    }
}