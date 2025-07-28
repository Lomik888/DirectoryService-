using CSharpFunctionalExtensions;

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

    public static Result<PositionName, IEnumerable<Error.Error>> Create(string value)
    {
        var errors = new List<Error.Error>();

        if (string.IsNullOrWhiteSpace(value))
        {
            var error = Error.Error.Create("Имя позиции не может быть пустым");
            errors.Add(error);
        }

        if (value.Length < NAME_MIN_LENGHT || value.Length > NAME_MAX_LENGHT)
        {
            var error = Error.Error.Create($"Имя позиции должно быть {NAME_MIN_LENGHT}-{NAME_MAX_LENGHT} симвалов");
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