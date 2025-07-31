using DirectoryService.Domain.Abstractions;

namespace DirectoryService.Application.Abstractions;

public class Clock : IClock
{
    public DateTime UtcNow()
    {
        return DateTime.UtcNow;
    }
}