using System.Text.Json;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Err;
using DirectoryService.Domain.Extations;

namespace DirectoryService.Domain.LocationValueObjects;

public class Address : ValueObject
{
    public string City { get; init; }

    public string Street { get; init; }

    public string HouseNumber { get; init; }

    public string? Number { get; init; }

    public string FullAddress(string locationName) => $"{locationName} {City} {Street} {HouseNumber} {Number}";

    public string ToJson() => JsonSerializer.Serialize(this, JsonSerializerOptions.Default);

    private Address(
        string city,
        string street,
        string houseNumber,
        string? number)
    {
        City = city;
        Street = street;
        HouseNumber = houseNumber;
        Number = number;
    }

    public static Result<Address, IEnumerable<Error>> Create(
        string city,
        string street,
        string houseNumber,
        string? number)
    {
        var errors = Validate(city, street, houseNumber, number);

        if (errors.Count > 0)
            return errors;

        return new Address(city, street, houseNumber, number);
    }

    public static List<Error> Validate(
        string city,
        string street,
        string houseNumber,
        string? number)
    {
        string[] values = number == null ? [city, street, houseNumber] : [city, street, houseNumber, number];

        var errors = StringExtations.Validation.IsNullOrWhiteSpace(values).ToList();
        return errors;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return City;
        yield return Street;
        yield return HouseNumber;
        if (Number != null)
            yield return Number;
    }
}