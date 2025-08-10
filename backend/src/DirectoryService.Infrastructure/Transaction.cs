using System.Data.Common;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Domain.Err;
using DirectoryService.Domain.Extations;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure;

public class Transaction : ITransaction
{
    private readonly DbTransaction _dbTransaction;
    private readonly ILogger<Transaction> _logger;

    public Transaction(DbTransaction dbTransaction, ILogger<Transaction> logger)
    {
        _dbTransaction = dbTransaction;
        _logger = logger;
    }

    public async Task<UnitResult<Errors>> CommitAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dbTransaction.CommitAsync(cancellationToken);
            _logger.LogInformation("Transaction commited");

            return UnitResult.Success<Errors>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            var error = GeneralErrors.Database.CommitError(ex.Message);

            return Errors.Create(error.ToList());
        }
    }

    public async Task<UnitResult<Errors>> RollbackAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dbTransaction.RollbackAsync(cancellationToken);
            _logger.LogInformation("Transaction Rollback completed");

            return UnitResult.Success<Errors>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            var error = GeneralErrors.Database.RollbackError(ex.Message);

            return Errors.Create(error.ToList());
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _dbTransaction.DisposeAsync();
    }
}