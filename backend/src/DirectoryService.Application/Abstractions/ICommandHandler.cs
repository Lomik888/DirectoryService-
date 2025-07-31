using CSharpFunctionalExtensions;

namespace DirectoryService.Application.Abstractions;

public interface ICommandHandler<TResult, TError, TCommand>
    where TCommand : ICommand
{
    public Task<Result<TResult, TError>> HandleAsync(
        TCommand command,
        CancellationToken cancellationToken);
}

public interface ICommandHandler<TError, TCommand>
    where TCommand : ICommand
{
    public Task<UnitResult<TError>> HandleAsync(
        TCommand command,
        CancellationToken cancellationToken);
}