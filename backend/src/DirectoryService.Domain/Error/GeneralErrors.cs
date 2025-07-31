namespace DirectoryService.Domain.Error;

public static class GeneralErrors
{
    public static class Validation
    {
        public static Error InvalidField(string message, string code)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Сообщение ошибки не может быть пустым");

            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Код ошибки не может быть пустым");

            return Error.Create(message, code, ErrorTypes.VALIDATION);
        }
    }
}