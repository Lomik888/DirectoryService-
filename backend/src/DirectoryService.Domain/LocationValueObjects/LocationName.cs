using CSharpFunctionalExtensions;
using DirectoryService.Domain.Err;

namespace DirectoryService.Domain.LocationValueObjects;

public class LocationName : ValueObject
{
    public const int NAME_MIN_LENGHT = 3;
    public const int NAME_MAX_LENGHT = 120;

    public string Value { get; init; }

    private LocationName(string value)
    {
        Value = value;
    }

    public static Result<LocationName, IEnumerable<Error>> Create(string value)
    {
        var errors = Validate(value);

        if (errors.Count > 0)
            return errors;

        return new LocationName(value);
    }

    public static List<Error> Validate(string value)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(value))
        {
            errors.Add(Error.Create(
                "Имя локации не может быть пустым",
                "invalid.parameter",
                ErrorTypes.VALIDATION));
        }

        if (value.Length < NAME_MIN_LENGHT || value.Length > NAME_MAX_LENGHT)
        {
            errors.Add(Error.Create(
                $"Имя локации должно быть {NAME_MIN_LENGHT}-{NAME_MAX_LENGHT} символов",
                "invalid.parameter",
                ErrorTypes.VALIDATION));
        }

        return errors;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}