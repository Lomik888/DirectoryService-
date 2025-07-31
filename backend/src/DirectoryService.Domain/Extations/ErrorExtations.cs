namespace DirectoryService.Domain.Extations;

public static class ErrorExtations
{
    public static List<Error.Error> ToList(this Error.Error error)
    {
        var list = new List<Error.Error>() { error };
        return list;
    }
}