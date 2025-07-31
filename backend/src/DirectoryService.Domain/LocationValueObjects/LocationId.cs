using CSharpFunctionalExtensions;
using DirectoryService.Domain.DepartmentValueObjects;
using DirectoryService.Domain.Error;

namespace DirectoryService.Domain.LocationValueObjects;

public class LocationId : ComparableValueObject
{
    public Guid Value { get; init; }

    private LocationId(Guid value)
    {
        Value = value;
    }

    public static LocationId Create() => new LocationId(Guid.NewGuid());

    public static Result<LocationId, Error.Error> Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            return Error.Error.Create(
                "Guid пустой",
                "invalid.parameter",
                ErrorTypes.VALIDATION);
        }

        return new LocationId(value);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}