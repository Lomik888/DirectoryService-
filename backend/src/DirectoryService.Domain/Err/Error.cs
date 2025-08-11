namespace DirectoryService.Domain.Err;

public class Error
{
    private const string SEPARATOR = " | ";

    private const int COUNT_PARAMS = 3;

    public string Message { get; private set; }

    public string Code { get; private set; }

    public ErrorTypes Type { get; private set; }

    private Error(string message, string code, ErrorTypes type)
    {
        Message = message;
        Code = code;
        Type = type;
    }

    public static Error Create(string message, string code, ErrorTypes type)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentNullException(nameof(message), "Сообщение в ошибке не может быть пустым");
        }

        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentNullException(nameof(code), "Код в ошибке не может быть пустым");
        }

        return new Error(message, code, type);
    }

    public string Serialize()
    {
        var errorString = string.Join(SEPARATOR, this.Message, this.Code, this.Type);
        return errorString;
    }

    public static Error Deserialize(string errorString)
    {
        if (string.IsNullOrWhiteSpace(errorString))
            throw new AggregateException("errorString не может быть пустым или null");

        var paramsError = errorString.Split(SEPARATOR);

        if (paramsError.Length != COUNT_PARAMS)
            throw new AggregateException("errorString не Error");

        if (Enum.TryParse<ErrorTypes>(paramsError[COUNT_PARAMS - 1], out var errorType) == false)
            throw new AggregateException("ErrorType invalid");

        var error = new Error(paramsError[0], paramsError[1], errorType);

        return error;
    }
}