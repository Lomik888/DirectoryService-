namespace DirectoryService.Application.Abstractions;

public interface IClock
{
    DateTime UtcNow();
}