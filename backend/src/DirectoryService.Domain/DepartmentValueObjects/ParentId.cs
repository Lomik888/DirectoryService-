using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.DepartmentValueObjects;

public class ParentId : ValueObject
{
    public Guid Value { get; init; }

    private ParentId(Guid value)
    {
        Value = value;
    }

    public static Result<ParentId, Error.Error> Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            return Error.Error.Create("Guid пустой");
        }

        return new ParentId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}