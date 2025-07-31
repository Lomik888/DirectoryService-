using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.DepartmentValueObjects;

public class DepartmentId : ComparableValueObject
{
    public Guid Value { get; init; }

    private DepartmentId(Guid value)
    {
        Value = value;
    }

    public static DepartmentId Create() => new DepartmentId(Guid.NewGuid());

    public static Result<DepartmentId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            return Error.Create("Guid пустой");
        }

        return new DepartmentId(value);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}