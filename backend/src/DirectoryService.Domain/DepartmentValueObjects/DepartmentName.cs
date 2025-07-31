using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.DepartmentValueObjects;

public class DepartmentName : ValueObject
{
    public const int NAME_MIN_LENGHT = 3;
    public const int NAME_MAX_LENGHT = 150;

    public string Value { get; init; }

    private DepartmentName(string value)
    {
        Value = value;
    }

    public static Result<DepartmentName, IEnumerable<Error>> Create(string value)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(value))
        {
            var error = Error.Create("Имя департамента не может быть пустым");
            errors.Add(error);
        }

        if (value.Length < NAME_MIN_LENGHT || value.Length > NAME_MAX_LENGHT)
        {
            var error = Error.Create($"Имя департамента должно быть {NAME_MIN_LENGHT}-{NAME_MAX_LENGHT} симвалов");
            errors.Add(error);
            return errors;
        }

        return new DepartmentName(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}