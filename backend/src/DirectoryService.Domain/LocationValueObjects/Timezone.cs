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
        if (string.IsNullOrWhiteSpace(value))
        {
            return Error.Error.Create(
                "Значение временной зоны не может быть пустым",
                "invalid.parameter",
                ErrorTypes.VALIDATION).ToList();
        }

        var isValid = TimeZoneInfo.TryFindSystemTimeZoneById(value, out var _);

        if (isValid == false)
        {
            var error = Error.Error.Create(
                $"Временная зона с идентификатором '{value}' не найдена",
                "invalid.parameter",
                ErrorTypes.VALIDATION);
            return error.ToList();
        }

        return new Timezone(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}