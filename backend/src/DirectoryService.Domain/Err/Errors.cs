using System.Collections;
using DirectoryService.Domain.Extations;

namespace DirectoryService.Domain.Err;

public class Errors : IEnumerable<Error>
{
    private readonly List<Error> _errors;

    public IReadOnlyList<Error> Values => _errors;

    public Errors(List<Error> errors)
    {
        _errors = errors;
    }

    public static Errors Create(List<Error> errorsList)
    {
        var errors = new Errors(errorsList);
        return errors;
    }

    public IEnumerator<Error> GetEnumerator()
    {
        return _errors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static implicit operator Errors(List<Error> errors)
    {
        return new Errors(errors);
    }

    public static implicit operator Errors(Error error)
    {
        return new Errors(error.ToList());
    }
}