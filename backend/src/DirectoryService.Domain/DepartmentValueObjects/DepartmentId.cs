using CSharpFunctionalExtensions;
using DirectoryService.Domain.Error;

namespace DirectoryService.Domain.DepartmentValueObjects;

public class DepartmentId : ComparableValueObject
{
    public Guid Value { get; init; }

    private DepartmentId(Guid value)
    {
        Value = value;
    }

    public static DepartmentId Create() => new DepartmentId(Guid.NewGuid());

    public static Result<DepartmentId, Error.Error> Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            return Error.Error.Create("Guid пустой", "invalid.parameter", ErrorTypes.VALIDATION);
        }

        return new DepartmentId(value);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}