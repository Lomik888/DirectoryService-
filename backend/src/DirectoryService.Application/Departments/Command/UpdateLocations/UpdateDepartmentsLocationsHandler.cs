using System.Data;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Extations;
using DirectoryService.Domain.DepartmentValueObjects;
using DirectoryService.Domain.Err;
using DirectoryService.Domain.Extations;
using DirectoryService.Domain.LocationValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Departments.Command.UpdateLocations;

public class UpdateDepartmentsLocationsHandler : ICommandHandler<Guid, Errors, UpdateDepartmentsLocationsCommand>
{
    private readonly IValidator<UpdateDepartmentsLocationsCommand> _validator;
    private readonly ILogger<UpdateDepartmentsLocationsHandler> _logger;
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly ITransactionManager _transactionManager;

    public UpdateDepartmentsLocationsHandler(
        IValidator<UpdateDepartmentsLocationsCommand> validator,
        ILogger<UpdateDepartmentsLocationsHandler> logger,
        IDepartmentsRepository departmentsRepository,
        ITransactionManager transactionManager)
    {
        _validator = validator;
        _logger = logger;
        _departmentsRepository = departmentsRepository;
        _transactionManager = transactionManager;
    }

    public async Task<Result<Guid, Errors>> HandleAsync(
        UpdateDepartmentsLocationsCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("Invalid validation");
            return Errors.Create(validationResult.Errors.ToErrors());
        }

        var departmentId = DepartmentId.Create(command.DepartmentId).Value;
        var locationsIds = command.Request.LocationIds
            .Select(x => LocationId.Create(x).Value)
            .ToList();

        // Это чтобы у меня были консистентные данные в бд
        var transactionResult = await _transactionManager.BeginTransactionAsync(
            cancellationToken,
            IsolationLevel.Serializable);
        if (transactionResult.IsFailure == true)
        {
            return transactionResult.Error;
        }

        await using var transaction = transactionResult.Value;

        try
        {
            var locationsIsValidResult =
                await _validator.ValidateAsync(command, cancellation: cancellationToken, options: options =>
                    options.IncludeRuleSets(UpdateDepartmentsLocationsCommandValidator.DB_VALIDATION));
            if (locationsIsValidResult.IsValid == false)
            {
                var errors = Errors.Create(locationsIsValidResult.Errors.ToErrors());
                var rollbackResult = await transaction.RollbackAsync(cancellationToken);
                if (rollbackResult.IsFailure == true)
                {
                    errors.Add(rollbackResult.Error);
                    return errors;
                }

                return errors;
            }

            var departmentResult = await _departmentsRepository.GetByIdAsync(departmentId, cancellationToken);
            if (departmentResult.IsFailure == true)
            {
                var errors = departmentResult.Error;
                var rollbackResult = await transaction.RollbackAsync(cancellationToken);
                if (rollbackResult.IsFailure == true)
                {
                    errors.Add(rollbackResult.Error);
                    return errors;
                }

                return errors;
            }

            var department = departmentResult.Value;

            department.AddLocationsWithClear(locationsIds);

            var saveChangesResult = await _transactionManager.SaveChangesAsync(cancellationToken);
            if (saveChangesResult.IsFailure == true)
            {
                var errors = saveChangesResult.Error;
                var rollbackResult = await transaction.RollbackAsync(cancellationToken);
                if (rollbackResult.IsFailure == true)
                {
                    errors.Add(rollbackResult.Error);
                    return errors;
                }

                return errors;
            }

            var commitResult = await transaction.CommitAsync(cancellationToken);
            if (commitResult.IsFailure == true)
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

            return departmentId.Value;
        }
        catch (Exception ex)
        {
            var errors = GeneralErrors.Database.TransactionError(ex.Message).ToErrors();
            var rollbackResult = await transaction.RollbackAsync(cancellationToken);
            if (rollbackResult.IsFailure == true)
            {
                errors.Add(rollbackResult.Error);
                return errors;
            }

            _logger.LogError(ex, "Error during transaction");

            return errors;
        }
    }
}