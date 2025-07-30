namespace DirectoryService.Domain.Extations;

public static class StringExtations
{
    public static class Validation
    {
        public static IEnumerable<Error.Error> IsNullOrWhiteSpace(IEnumerable<string> strings)
        {
            var errors = new List<Error.Error>();

            foreach (var @string in strings)
            {
                if (string.IsNullOrWhiteSpace(@string) == true)
                {
                    Error.Error error = Error.Error.Create($"Строка {nameof(@string)} пустая или null");
                    errors.Add(error);
                }
            }

            return errors;
        }
    }
}