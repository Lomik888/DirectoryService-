using CSharpFunctionalExtensions;
using DirectoryService.Domain.Err;

namespace DirectoryService.Application.Abstractions;

public interface ITransaction : IAsyncDisposable
{
    Task<UnitResult<Errors>> CommitAsync(CancellationToken cancellationToken);

    Task<UnitResult<Errors>> RollbackAsync(CancellationToken cancellationToken);
}