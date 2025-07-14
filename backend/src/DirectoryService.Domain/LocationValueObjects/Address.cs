using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.LocationValueObjects;

public class Address : ValueObject
{
    public string Value { get; init; }

    private Address(string value)
    {
        Value = value;
    }

    public static Result<Address, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            var error = Error.Create($"Адрес не может быть пустым");
            return error;
        }

        return new Address(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}