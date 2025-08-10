using System.Data;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Domain.Error;
using DirectoryService.Domain.Extations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure;

public class TransactionManager<T> : ITransactionManager
    where T : DbContext
{
    private readonly T _dbContext;
    private readonly ILogger<TransactionManager<T>> _logger;
    private readonly ILoggerFactory _loggerFactory;

    public TransactionManager(
        T dbContext,
        ILogger<TransactionManager<T>> logger,
        ILoggerFactory loggerFactory)
    {
        _dbContext = dbContext;
        _logger = logger;
        _loggerFactory = loggerFactory;
    }

    public async Task<Result<ITransaction, Errors>> BeginTransactionAsync(
        CancellationToken cancellationToken,
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        try
        {
            await using var dbTransaction = await _dbContext.Database.BeginTransactionAsync(
                isolationLevel,
                cancellationToken);

            var transaction = new Transaction(
                dbTransaction.GetDbTransaction(),
                _loggerFactory.CreateLogger<Transaction>());

            return transaction;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Errors.Create(GeneralErrors.Database.TransactionError(ex.Message).ToList());
        }
    }

    public async Task<UnitResult<Errors>> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return UnitResult.Success<Errors>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Errors.Create(GeneralErrors.Database.TransactionError(ex.Message).ToList());
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }
}