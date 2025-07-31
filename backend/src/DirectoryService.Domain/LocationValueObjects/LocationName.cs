using CSharpFunctionalExtensions;
using DirectoryService.Domain.Error;

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

    public static Result<LocationName, IEnumerable<Error.Error>> Create(string value)
    {
        var errors = new List<Error.Error>();

        if (string.IsNullOrWhiteSpace(value))
        {
            var error = Error.Error.Create(
                "Имя локации не может быть пустым",
                "invalid.parameter",
                ErrorTypes.VALIDATION);
            errors.Add(error);
        }

        if (value.Length < NAME_MIN_LENGHT || value.Length > NAME_MAX_LENGHT)
        {
            var error = Error.Error.Create(
                $"Имя локации должно быть {NAME_MIN_LENGHT}-{NAME_MAX_LENGHT} симвалов",
                "invalid.parameter",
                ErrorTypes.VALIDATION);
            errors.Add(error);
            return errors;
        }

        return new LocationName(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}