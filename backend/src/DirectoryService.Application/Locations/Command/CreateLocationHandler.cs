using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Extations;
using DirectoryService.Domain;
using DirectoryService.Domain.Abstractions;
using DirectoryService.Domain.Err;
using DirectoryService.Domain.Extations;
using DirectoryService.Domain.LocationValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Locations.Command;

public class CreateLocationHandler : ICommandHandler<Guid, Errors, CreateLocationCommand>
{
    private readonly IValidator<CreateLocationCommand> _validator;
    private readonly ILogger<CreateLocationHandler> _logger;
    private readonly IClock _clock;
    private readonly ILocationRepository _locationRepository;

    public CreateLocationHandler(
        IValidator<CreateLocationCommand> validator,
        ILogger<CreateLocationHandler> logger,
        ILocationRepository locationRepository,
        IClock clock)
    {
        _validator = validator;
        _logger = logger;
        _locationRepository = locationRepository;
        _clock = clock;
    }

    public async Task<Result<Guid, Errors>> HandleAsync(
        CreateLocationCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("Invalid validation");
            return Errors.Create(validationResult.Errors.ToErrors());
        }

        var timeZone = Timezone.Create(command.Request.TimeZone).Value;
        var locationName = LocationName.Create(command.Request.LocationName).Value;
        var address = Address.Create(
            command.Request.City,
            command.Request.Street,
            command.Request.HouseNumber,
            command.Request.Number).Value;

        var locationExists =
            await _locationRepository.LocationExistsAsync(
                locationName,
                address,
                cancellationToken);
        if (locationExists == true)
        {
            var errors = GeneralErrors.Validation.AllreadyExists(
                $"Локаия {locationName.Value} {address.FullAddress(locationName.Value)} уже сущестует",
                "location.existentialists");

            return Errors.Create(errors.ToList());
        }

        var locationResult = CreateLocation(
            locationName,
            timeZone,
            address,
            _clock);
        if (locationResult.IsFailure == true)
        {
            var errors = GeneralErrors.Validation.CanNotCreate("Невозможно создать локацию", "create.location");

            return Errors.Create(errors.ToList());
        }

        var location = locationResult.Value;

        await _locationRepository.AddAsync(location, cancellationToken);
        await _locationRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created location ID : {0}", location.Id.Value);
        return location.Id.Value;
    }

    private Result<Location, Errors> CreateLocation(
        LocationName locationName,
        Timezone timeZone,
        Address address,
        IClock clock)
    {
        var createLocationResult = Location.Create(locationName, timeZone, address, _clock);
        if (createLocationResult.IsFailure == true)
        {
            _logger.LogInformation("Can't create location");
            return Errors.Create(createLocationResult.Error.ToList());
        }

        return createLocationResult.Value;
    }
}