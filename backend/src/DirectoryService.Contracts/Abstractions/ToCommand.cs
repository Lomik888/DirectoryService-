namespace DirectoryService.Contracts.Abstractions;

public interface IToCommand<TParam, TCommand>
{
    public TCommand ToCommand(TParam param);
}

public interface IToCommand<TCommand>
{
    public TCommand ToCommand();
}