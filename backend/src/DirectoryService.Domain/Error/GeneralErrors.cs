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

        public static Error CanNotCreate(string message, string code)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Сообщение ошибки не может быть пустым");

            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Код ошибки не может быть пустым");

            return Error.Create(message, code, ErrorTypes.VALIDATION);
        }

        public static Error AllreadyExists(string message, string code)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Сообщение ошибки не может быть пустым");

            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Код ошибки не может быть пустым");

            return Error.Create(message, code, ErrorTypes.VALIDATION);
        }
    }

    public static class Database
    {
        public static Error CommitError(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Сообщение ошибки не может быть пустым");

            var code = "transaction.commit.error";

            return Error.Create(message, code, ErrorTypes.DB);
        }

        public static Error RollbackError(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Сообщение ошибки не может быть пустым");

            var code = "transaction.Rollback.error";

            return Error.Create(message, code, ErrorTypes.DB);
        }

        public static Error TransactionError(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Сообщение ошибки не может быть пустым");

            var code = "transaction.error";

            return Error.Create(message, code, ErrorTypes.DB);
        }
    }
}