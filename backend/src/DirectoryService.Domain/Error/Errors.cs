using System.Collections;

namespace DirectoryService.Domain.Error;

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
}