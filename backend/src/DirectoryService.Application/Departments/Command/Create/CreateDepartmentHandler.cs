using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Extations;
using DirectoryService.Application.Locations;
using DirectoryService.Domain;
using DirectoryService.Domain.Abstractions;
using DirectoryService.Domain.DepartmentValueObjects;
using DirectoryService.Domain.Err;
using DirectoryService.Domain.Extations;
using DirectoryService.Domain.LocationValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Departments.Command;

public class CreateDepartmentHandler : ICommandHandler<Guid, Errors, CreateDepartmentCommand>
{
    private readonly IValidator<CreateDepartmentCommand> _validator;
    private readonly ILogger<CreateDepartmentHandler> _logger;
    private readonly IClock _clock;
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly ILocationRepository _locationRepository;

    public CreateDepartmentHandler(
        IValidator<CreateDepartmentCommand> validator,
        ILogger<CreateDepartmentHandler> logger,
        IClock clock,
        IDepartmentsRepository departmentsRepository,
        ILocationRepository locationRepository)
    {
        _validator = validator;
        _logger = logger;
        _clock = clock;
        _departmentsRepository = departmentsRepository;
        _locationRepository = locationRepository;
    }

    public async Task<Result<Guid, Errors>> HandleAsync(
        CreateDepartmentCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("Invalid validation");
            return Errors.Create(validationResult.Errors.ToErrors());
        }

        var locationsIds = command.Request.LocationIds.Select(x => LocationId.Create(x).Value);

        var locationsNotExists = await _locationRepository.LocationsExistsAsync(locationsIds, cancellationToken);
        if (locationsNotExists.Count > 0)
        {
            var ids = locationsNotExists.Select(x => x.Value);
            var notExistsLocationsString = string.Join(" , ", ids);
            var error = GeneralErrors.Validation.NotFound(
                $"Таких локаций нет {notExistsLocationsString}",
                "locations.not.found");

            return error.ToErrors();
        }

        var addedDepartmentIdResult = command.Request.ParentId == null
            ? await CreateParent(command, cancellationToken)
            : await CreateChild(command, cancellationToken);
        if (addedDepartmentIdResult.IsFailure == true)
        {
            return addedDepartmentIdResult.Error;
        }

        var addedDepartmentId = addedDepartmentIdResult.Value;

        return addedDepartmentId.Value;
    }

    private async Task<Result<DepartmentId, Errors>> CreateParent(
        CreateDepartmentCommand command,
        CancellationToken cancellationToken)
    {
        if (command.Request.ParentId != null)
        {
            var error = GeneralErrors.Validation.InvalidField(
                "При создании родительского департамента в запросе указан id родительского департамента",
                "invalid.request.parent.id");

            return error.ToErrors();
        }

        var departmentName = DepartmentName.Create(command.Request.Name).Value;
        var identifier = Identifier.Create(command.Request.Identifier).Value;

        var departmentResult = Department.CreateParent(departmentName, identifier, _clock);
        if (departmentResult.IsFailure == true)
        {
            return departmentResult.Error.ToErrors();
        }

        var department = departmentResult.Value;
        AddLocations(department, command.Request.LocationIds);

        await _departmentsRepository.AddAsync(department, cancellationToken);
        await _departmentsRepository.SaveChangesAsync(cancellationToken);

        return department.Id;
    }

    private async Task<Result<DepartmentId, Errors>> CreateChild(
        CreateDepartmentCommand command,
        CancellationToken cancellationToken)
    {
        if (command.Request.ParentId == null)
        {
            var error = GeneralErrors.Validation.InvalidField(
                "При создании дочернего департамента в запросе указан не указан id родителя",
                "invalid.request.parent.id");

            return error.ToErrors();
        }

        var departmentId = DepartmentId.Create((Guid)command.Request.ParentId!).Value;

        var parentResult =
            await _departmentsRepository.GetByIdAsync(departmentId, cancellationToken);
        if (parentResult.IsFailure == true)
        {
            return parentResult.Error;
        }

        var parent = parentResult.Value;
        var departmentName = DepartmentName.Create(command.Request.Name).Value;
        var identifier = Identifier.Create(command.Request.Identifier).Value;

        var childResult = parent.AddChild(departmentName, identifier, _clock);
        if (childResult.IsFailure == true)
        {
            return childResult.Error.ToErrors();
        }

        var child = childResult.Value;
        AddLocations(child, command.Request.LocationIds);

        await _departmentsRepository.AddAsync(child, cancellationToken);
        await _departmentsRepository.SaveChangesAsync(cancellationToken);

        return child.Id;
    }

    private void AddLocations(Department department, List<Guid> locationIds)
    {
        var locationsIds = locationIds.Select(x => LocationId.Create(x).Value).ToList();

        department.AddLocations(locationsIds);
    }
}