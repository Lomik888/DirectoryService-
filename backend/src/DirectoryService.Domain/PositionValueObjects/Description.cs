using CSharpFunctionalExtensions;
using DirectoryService.Domain.Error;

namespace DirectoryService.Domain.PositionValueObjects;

public class Description : ValueObject
{
    public const int DESCRIPTION_MAX_LENGHT = 1000;

    public string Value { get; init; }

    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description, Error.Error> Create(string value)
    {
        if (value.Length > DESCRIPTION_MAX_LENGHT)
        {
            var error = Error.Error.Create(
                $"Описание должно быть не больше {DESCRIPTION_MAX_LENGHT} симвалов",
                "invalid.parameter",
                ErrorTypes.VALIDATION);
            return error;
        }

        return new Description(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}