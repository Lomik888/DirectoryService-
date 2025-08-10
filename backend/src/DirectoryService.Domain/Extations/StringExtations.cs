using DirectoryService.Domain.Err;

namespace DirectoryService.Domain.Extations;

public static class StringExtations
{
    public static class Validation
    {
        public static IEnumerable<Error> IsNullOrWhiteSpace(IEnumerable<string> strings)
        {
            var errors = new List<Error>();

            foreach (var @string in strings)
            {
                if (string.IsNullOrWhiteSpace(@string) == true)
                {
                    Error error = Error.Create(
                        $"Строка {nameof(@string)} пустая или null",
                        "invalid.parameter",
                        ErrorTypes.VALIDATION);
                    errors.Add(error);
                }
            }

            return errors;
        }
    }
}