using CSharpFunctionalExtensions;
using DirectoryService.Domain.Error;
using DirectoryService.Domain.Extations;

namespace DirectoryService.Domain.LocationValueObjects;

public class Timezone : ValueObject
{
    public string Value { get; init; }

    private Timezone(string value)
    {
        Value = value;
    }

    public static Result<Timezone, IEnumerable<Error.Error>> Create(string value)
    {
        var errors = Validate(value);

        if (errors.Count > 0)
            return errors;

        return new Timezone(value);
    }

    public static List<Error.Error> Validate(string value)
    {
        var errors = new List<Error.Error>();

        if (string.IsNullOrWhiteSpace(value))
        {
            errors.Add(Error.Error.Create(
                "Значение временной зоны не может быть пустым",
                "invalid.parameter",
                ErrorTypes.VALIDATION));
            return errors;
        }

        var isValid = TimeZoneInfo.TryFindSystemTimeZoneById(value, out _);

        if (!isValid)
        {
            errors.Add(Error.Error.Create(
                $"Временная зона с идентификатором '{value}' не найдена",
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