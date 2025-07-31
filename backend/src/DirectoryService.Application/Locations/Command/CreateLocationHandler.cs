using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Extations;
using DirectoryService.Domain;
using DirectoryService.Domain.Abstractions;
using DirectoryService.Domain.Error;
using DirectoryService.Domain.Extations;
using DirectoryService.Domain.LocationValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Locations.Command;

public class CreateLocationHandler : ICommandHandler<Guid, List<Error>, CreateLocationCommand>
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

    public async Task<Result<Guid, List<Error>>> HandleAsync(
        CreateLocationCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("Invalid validation");
            return validationResult.Errors.ToErrors();
        }

        var locationName = LocationName.Create(command.Request.LocationName).Value;
        var timeZone = Timezone.Create(command.Request.TimeZone).Value;
        var address = Address.Create(
            command.Request.City,
            command.Request.Street,
            command.Request.HouseNumber,
            command.Request.Number).Value;

        var createLocationResult = Location.Create(locationName, timeZone, address, _clock);
        if (createLocationResult.IsFailure == true)
        {
            _logger.LogInformation("Can't create location");
            return createLocationResult.Error.ToList();
        }

        var location = createLocationResult.Value;

        var addAddressResult = location.AddAddress(address);
        if (addAddressResult.IsFailure == true)
        {
            _logger.LogInformation("Can't added address to location");
            return addAddressResult.Error.ToList();
        }

        await _locationRepository.AddAsync(location, cancellationToken);
        await _locationRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created location ID : {0}", location.Id);
        return location.Id.Value;
    }
}