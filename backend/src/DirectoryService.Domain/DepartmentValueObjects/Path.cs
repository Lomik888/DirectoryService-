using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Err;
using DirectoryService.Domain.Extations;

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

    public static Result<Path, Errors> CreateParent(Path path, Identifier identifier)
    {
        var departmentPathStrings = path.Value
            .Split(Separator)
            .ToList();

        string newPath;
        Path newDepartmentPath;

        var indexOldParent = departmentPathStrings.IndexOf(identifier.Value);
        if (indexOldParent == -1)
        {
            var error = GeneralErrors.Validation.InvalidField(
                "departmentIdentifier.Value не найден в department.Value",
                "department.invalid.path");
            return error.ToErrors();
        }

        if (indexOldParent == 0)
        {
            return path;
        }

        var newDepartmentPathStrings = departmentPathStrings.Skip(indexOldParent - 1);
        newPath = string.Join(Separator, newDepartmentPathStrings);
        newDepartmentPath = new Path(newPath);

        return newDepartmentPath;
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

    public static Result<Path, Errors> Create(Path parent, Path department, Identifier departmentIdentifier)
    {
        var departmentPathStrings = department.Value
            .Split(Separator)
            .ToList();

        string newPath;
        Path newDepartmentPath;

        var indexOldParent = departmentPathStrings.IndexOf(departmentIdentifier.Value);
        if (indexOldParent == -1)
        {
            var error = GeneralErrors.Validation.InvalidField(
                "departmentIdentifier.Value не найден в department.Value",
                "department.invalid.path");
            return error.ToErrors();
        }

        if (indexOldParent == 0)
        {
            newPath = $"{parent.Value}{department.Value}";
            newDepartmentPath = new Path(newPath);

            return newDepartmentPath;
        }

        var newDepartmentPathStrings = departmentPathStrings.Skip(indexOldParent - 1);
        var newDepartmentPathString = string.Join(Separator, newDepartmentPathStrings);
        newPath = $"{parent.Value}{newDepartmentPathString}";
        newDepartmentPath = new Path(newPath);

        return newDepartmentPath;
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