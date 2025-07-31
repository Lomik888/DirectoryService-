using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Error;

namespace DirectoryService.Domain.DepartmentValueObjects;

public class Path : ValueObject
{
    private static readonly Regex _pathRegex = new Regex(@"^[a-z0-9.-]+$", RegexOptions.IgnoreCase);

    public static readonly char Separator = '.';

    public string Value { get; init; }

    private Path(string value)
    {
        Value = value;
    }

    public static Result<Path, IEnumerable<Error.Error>> Create(string value)
    {
        var errors = new List<Error.Error>();

        if (string.IsNullOrWhiteSpace(value))
        {
            var error = Error.Error.Create(
                "Путь департамента не может быть пустым",
                "invalid.parameter",
                ErrorTypes.VALIDATION);
            errors.Add(error);
        }

        if (_pathRegex.IsMatch(value) == false)
        {
            var error = Error.Error.Create(
                $"Путь департамента невалидный",
                "invalid.parameter",
                ErrorTypes.VALIDATION);
            errors.Add(error);
            return errors;
        }

        return new Path(value);
    }

    public short GetDepth()
    {
        short depth = (short)this.Value.Split(Separator).Length;
        return depth;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}