using System.Data;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Error;

namespace DirectoryService.Application.Abstractions;

public interface ITransactionManager : IAsyncDisposable
{
    Task<Result<ITransaction, Errors>> BeginTransactionAsync(
        CancellationToken cancellationToken,
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

    Task<UnitResult<Errors>> SaveChangesAsync(CancellationToken cancellationToken);
}