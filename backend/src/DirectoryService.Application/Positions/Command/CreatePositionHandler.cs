using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Extations;
using DirectoryService.Domain;
using DirectoryService.Domain.Abstractions;
using DirectoryService.Domain.DepartmentValueObjects;
using DirectoryService.Domain.Err;
using DirectoryService.Domain.Extations;
using DirectoryService.Domain.PositionValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Positions.Command;

public class CreatePositionHandler : ICommandHandler<Guid, Errors, CreatePositionCommand>
{
    private readonly IValidator<CreatePositionCommand> _validator;
    private readonly ILogger<CreatePositionHandler> _logger;
    private readonly IClock _clock;
    private readonly IPositionRepository _positionRepository;

    public CreatePositionHandler(
        IValidator<CreatePositionCommand> validator,
        ILogger<CreatePositionHandler> logger,
        IClock clock,
        IPositionRepository positionRepository)
    {
        _validator = validator;
        _logger = logger;
        _clock = clock;
        _positionRepository = positionRepository;
    }

    public async Task<Result<Guid, Errors>> HandleAsync(
        CreatePositionCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("Invalid validation");
            return Errors.Create(validationResult.Errors.ToErrors());
        }

        var positionName = PositionName.Create(command.Request.Name).Value;
        var description = command.Request.Description == null
            ? null
            : Description.Create(command.Request.Description).Value;

        var positionResult = Position.Create(positionName, description, _clock);
        if (positionResult.IsFailure == true)
        {
            return positionResult.Error.ToErrors();
        }

        var departments = command.Request.DepartmentIds.Select(x => DepartmentId.Create(x).Value);

        var position = positionResult.Value;
        position.AddDepartments(departments);

        await _positionRepository.AddAsync(position, cancellationToken);
        await _positionRepository.SaveChangesAsync(cancellationToken);

        return position.Id.Value;
    }
}