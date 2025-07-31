namespace DirectoryService.Domain;

public class Error
{
    public string Message { get; private set; }

    private Error(string message)
    {
        Message = message;
    }

    public static Error Create(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentNullException(nameof(message), "Сообщение в ошибке не может быть пустым");
        }

        return new Error(message);
    }
}