namespace DirectoryService.Domain.Abstractions;

public interface IClock
{
    DateTime UtcNow();
}