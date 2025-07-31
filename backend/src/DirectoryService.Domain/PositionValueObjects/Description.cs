using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.PositionValueObjects;

public class Description : ValueObject
{
    public const int DESCRIPTION_MAX_LENGHT = 1000;

    public string Value { get; init; }

    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description, Error> Create(string value)
    {
        if (value.Length > DESCRIPTION_MAX_LENGHT)
        {
            var error = Error.Create($"Описание должно быть не больше {DESCRIPTION_MAX_LENGHT} симвалов");
            return error;
        }

        return new Description(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}