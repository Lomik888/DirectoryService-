using CSharpFunctionalExtensions;
using DirectoryService.Domain.Error;

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
            return Error.Error.Create(
                "Guid пустой",
                "invalid.parameter",
                ErrorTypes.VALIDATION);
        }

        return new ParentId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}