using DirectoryService.Domain.Err;

namespace DirectoryService.Domain.Extations;

public static class ErrorExtations
{
    public static List<Error> ToList(this Error error)
    {
        var list = new List<Error>() { error };
        return list;
    }

    public static Errors ToErrors(this Error error)
    {
        var errors = Errors.Create(error.ToList());
        return errors;
    }
}