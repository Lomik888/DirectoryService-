using CSharpFunctionalExtensions;
using DirectoryService.Domain.Err;

namespace DirectoryService.Domain.PositionValueObjects;

public class PositionName : ValueObject
{
    public const int NAME_MIN_LENGHT = 3;
    public const int NAME_MAX_LENGHT = 100;

    public string Value { get; init; }

    private PositionName(string value)
    {
        Value = value;
    }

    public static Result<PositionName, IEnumerable<Error>> Create(string value)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(value))
        {
            var error = Error.Create(
                "Имя позиции не может быть пустым",
                "invalid.parameter",
                ErrorTypes.VALIDATION);
            errors.Add(error);
        }

        if (value.Length < NAME_MIN_LENGHT || value.Length > NAME_MAX_LENGHT)
        {
            var error = Error.Create(
                $"Имя позиции должно быть {NAME_MIN_LENGHT}-{NAME_MAX_LENGHT} симвалов",
                "invalid.parameter",
                ErrorTypes.VALIDATION);
            errors.Add(error);
            return errors;
        }

        return new PositionName(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}