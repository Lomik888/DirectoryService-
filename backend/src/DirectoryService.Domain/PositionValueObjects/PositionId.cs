using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.PositionValueObjects;

public class PositionId : ComparableValueObject
{
    public Guid Value { get; init; }

    private PositionId(Guid value)
    {
        Value = value;
    }

    public static PositionId Create() => new PositionId(Guid.NewGuid());

    public static Result<PositionId, Error.Error> Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            return Error.Error.Create("Guid пустой");
        }

        return new PositionId(value);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}