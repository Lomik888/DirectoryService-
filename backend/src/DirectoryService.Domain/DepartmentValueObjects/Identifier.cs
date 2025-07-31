using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.DepartmentValueObjects;

public class Identifier : ValueObject
{
    public const int IDENTIFIER_MIN_LENGHT = 3;
    public const int IDENTIFIER_MAX_LENGHT = 150;

    public string Value { get; init; }

    private Identifier(string value)
    {
        Value = value;
    }

    public static Result<Identifier, IEnumerable<Error>> Create(string value)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(value))
        {
            var error = Error.Create("Идентификатор не может быть пустым");
            errors.Add(error);
        }

        if (value.Length < IDENTIFIER_MIN_LENGHT || value.Length > IDENTIFIER_MAX_LENGHT)
        {
            var error =
                Error.Create($"Идентификатор должно быть {IDENTIFIER_MIN_LENGHT}-{IDENTIFIER_MAX_LENGHT} симвалов");
            errors.Add(error);
            return errors;
        }

        return new Identifier(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}