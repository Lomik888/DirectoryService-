using System.Data;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Extations;
using DirectoryService.Domain;
using DirectoryService.Domain.Abstractions;
using DirectoryService.Domain.DepartmentValueObjects;
using DirectoryService.Domain.Err;
using DirectoryService.Domain.Extations;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Path = DirectoryService.Domain.DepartmentValueObjects.Path;

namespace DirectoryService.Application.Departments.Command.Transfer;

public class TransferDepartmentHandler : ICommandHandler<Errors, TransferDepartmentCommand>
{
    private readonly IValidator<TransferDepartmentCommand> _validator;
    private readonly ILogger<TransferDepartmentHandler> _logger;
    private readonly IClock _clock;
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly ITransactionManager _transactionManager;

    public TransferDepartmentHandler(
        IValidator<TransferDepartmentCommand> validator,
        ILogger<TransferDepartmentHandler> logger,
        IClock clock,
        IDepartmentsRepository departmentsRepository,
        ITransactionManager transactionManager)
    {
        _validator = validator;
        _logger = logger;
        _clock = clock;
        _departmentsRepository = departmentsRepository;
        _transactionManager = transactionManager;
    }

    public async Task<UnitResult<Errors>> HandleAsync(
        TransferDepartmentCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("Invalid validation");
            return Errors.Create(validationResult.Errors.ToErrors());
        }

        var transactionResult = await _transactionManager.BeginTransactionAsync(
            cancellationToken,
            IsolationLevel.Serializable);
        if (transactionResult.IsFailure == true)
        {
            return transactionResult.Error;
        }

        await using var transaction = transactionResult.Value;

        // получил chi and par
        var getEntitiesResult = await GetDepartmentAndParentAsync(command, cancellationToken);
        if (getEntitiesResult.IsFailure == true)
        {
            var errors = getEntitiesResult.Error;
            var rollbackResult = await transaction.RollbackAsync(cancellationToken);
            if (rollbackResult.IsFailure == true)
            {
                errors.Add(rollbackResult.Error);
                return errors;
            }

            return errors;
        }

        var (chi, par) = getEntitiesResult.Value;
        var oldPath = chi.Path;
        Path newPath;

        if (par == null)
        {
            // chi to par
            var result = MakeParentAsync(chi);
            if (result.IsFailure)
            {
                var errors = result.Error;
                var rollbackResult = await transaction.RollbackAsync(cancellationToken);
                if (rollbackResult.IsFailure == true)
                {
                    errors.Add(rollbackResult.Error);
                    return errors;
                }

                return errors;
            }
        }
        else
        {
            var result = ParentAddChildAsync(par, chi);
            if (result.IsFailure)
            {
                var errors = result.Error;
                var rollbackResult = await transaction.RollbackAsync(cancellationToken);
                if (rollbackResult.IsFailure == true)
                {
                    errors.Add(rollbackResult.Error);
                    return errors;
                }

                return errors;
            }
        }

        // у старого родителя убрал кол-во детей
        await _departmentsRepository.MinusChildrenCountAsync(
            chi.ParentId!,
            chi.ChildrenCount + 1,
            cancellationToken);

        newPath = chi.Path;
        await _departmentsRepository.UpdatePathsAndDephtsAsync(oldPath, newPath, cancellationToken);

        var transactionSaveResult = await _transactionManager.SaveChangesAsync(cancellationToken);
        if (transactionSaveResult.IsFailure)
        {
            var errors = transactionSaveResult.Error;
            var rollbackResult = await transaction.RollbackAsync(cancellationToken);
            if (rollbackResult.IsFailure == true)
            {
                errors.Add(rollbackResult.Error);
                return errors;
            }

            return errors;
        }

        var commitResult = await transaction.CommitAsync(cancellationToken);
        if (commitResult.IsFailure)
        {
            var errors = commitResult.Error;
            var rollbackResult = await transaction.RollbackAsync(cancellationToken);
            if (rollbackResult.IsFailure == true)
            {
                errors.Add(rollbackResult.Error);
                return errors;
            }

            return errors;
        }

        return UnitResult.Success<Errors>();
    }

    private UnitResult<Errors> ParentAddChildAsync(
        Department parent,
        Department child)
    {
        var addChildResult = parent.AddChild(child, _clock);
        if (addChildResult.IsFailure == true)
        {
            return addChildResult.Error;
        }

        return UnitResult.Success<Errors>();
    }

    private UnitResult<Errors> MakeParentAsync(
        Department department)
    {
        var makeParentResult = department.MakeParent(_clock);
        if (makeParentResult.IsFailure == true)
        {
            return makeParentResult.Error;
        }

        return UnitResult.Success<Errors>();
    }

    /// <summary>
    /// Получение dep и par если он не null
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>dep, par?</returns>
    private async Task<Result<(Department department, Department? parent), Errors>> GetDepartmentAndParentAsync(
        TransferDepartmentCommand command,
        CancellationToken cancellationToken)
    {
        var departmentResult = await GetDepartmentAsync(command, cancellationToken);
        if (departmentResult.IsFailure == true)
        {
            return departmentResult.Error;
        }

        var department = departmentResult.Value;

        if (command.ParentId == null)
        {
            return (department, null);
        }

        var parentResult = await GetParentAsync(command, cancellationToken);
        if (parentResult.IsFailure == true)
        {
            return parentResult.Error;
        }

        var parent = parentResult.Value;

        return (department, parent);
    }

    private async Task<Result<Department, Errors>> GetParentAsync(
        TransferDepartmentCommand command,
        CancellationToken cancellationToken)
    {
        var parentId = DepartmentId.Create((Guid)command.ParentId!).Value;

        var parentResult = await _departmentsRepository.GetByIdAsync(parentId, cancellationToken);
        if (parentResult.IsFailure == true)
        {
            return parentResult.Error;
        }

        var parent = parentResult.Value;

        return parent;
    }

    private async Task<Result<Department, Errors>> GetDepartmentAsync(
        TransferDepartmentCommand command,
        CancellationToken cancellationToken)
    {
        var departmentId = DepartmentId.Create(command.DepartmentId).Value;

        var departmentResult = await _departmentsRepository.GetByIdAsync(departmentId, cancellationToken);
        if (departmentResult.IsFailure == true)
        {
            return departmentResult.Error;
        }

        var department = departmentResult.Value;

        if (department.ParentId == null && command.ParentId == null)
        {
            var errors =
                GeneralErrors.Validation.CanNotCreate(
                    "Этот департамент уже является родителем",
                    "invalid.request");

            return errors.ToErrors();
        }

        if (department.Id.Value == command.ParentId)
        {
            var errors =
                GeneralErrors.Validation.CanNotCreate(
                    "Этот департамент уже является родителем",
                    "invalid.request");

            return errors.ToErrors();
        }

        if (department.ParentId?.Value == command.ParentId)
        {
            var errors =
                GeneralErrors.Validation.CanNotCreate(
                    "Этот департамент уже является дочерним для родителя",
                    "invalid.request");

            return errors.ToErrors();
        }

        return department;
    }
}