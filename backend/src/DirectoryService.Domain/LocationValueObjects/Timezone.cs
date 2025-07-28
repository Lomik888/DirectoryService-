using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.LocationValueObjects;

public class Timezone : ValueObject
{
    public string Value { get; init; }

    private Timezone(string value)
    {
        Value = value;
    }

    public static Result<Timezone, Error.Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Error.Error.Create("Значение временной зоны не может быть пустым");
        }

        var isValid = TimeZoneInfo.TryFindSystemTimeZoneById(value, out var _);

        if (isValid == false)
        {
            var error = Error.Error.Create($"Временная зона с идентификатором '{value}' не найдена");
            return error;
        }

        return new Timezone(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}