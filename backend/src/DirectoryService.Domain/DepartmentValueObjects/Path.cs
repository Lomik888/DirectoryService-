using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Err;

namespace DirectoryService.Domain.DepartmentValueObjects;

public class Path : ValueObject
{
    private static readonly Regex _pathRegex = new Regex(@"^[A-Za-z\- /]+$", RegexOptions.IgnoreCase);

    public static readonly char Separator = '/';

    public string Value { get; init; }

    private Path(string value)
    {
        Value = value;
    }

    public static Result<Path, IEnumerable<Error>> Create(string value)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(value))
        {
            var error = Error.Create(
                "Путь департамента не может быть пустым",
                "invalid.parameter",
                ErrorTypes.VALIDATION);
            errors.Add(error);
        }

        if (_pathRegex.IsMatch(value) == false)
        {
            var error = Error.Create(
                $"Путь департамента невалидный",
                "invalid.parameter",
                ErrorTypes.VALIDATION);
            errors.Add(error);
            return errors;
        }

        return new Path(value);
    }

    public static Result<Path, IEnumerable<Error>> Create(Path path, string value)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(value))
        {
            var error = Error.Create(
                "Путь департамента не может быть пустым",
                "invalid.parameter",
                ErrorTypes.VALIDATION);
            errors.Add(error);
        }

        if (_pathRegex.IsMatch(value) == false)
        {
            var error = Error.Create(
                $"Путь департамента невалидный",
                "invalid.parameter",
                ErrorTypes.VALIDATION);
            errors.Add(error);
            return errors;
        }

        var newPathString = $"{path.Value}{value}/";

        return new Path(newPathString);
    }


    public UnitResult<Error> ValidateChildPath(Identifier identifier)
    {
        var identifierExists = this.Value.Split(Separator).Contains(identifier.Value);
        if (identifierExists == true)
        {
            var error =
                GeneralErrors.Validation.InvalidField("identifier already exists.", "invalid.identifier");
            return error;
        }

        return UnitResult.Success<Error>();
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