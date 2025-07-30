using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Domain;
using DirectoryService.Domain.Error;
using DirectoryService.Domain.LocationValueObjects;

namespace DirectoryService.Application.Factories;

public class LocationFactories
{
    private IClock _clock;

    public LocationFactories(IClock clock)
    {
        _clock = clock;
    }

    public Result<Location, Error> Create(
        LocationName name,
        Timezone timezone)
    {
        var utcNow = _clock.UtcNow();

        var createLocationResult = Location.Create(name, timezone, utcNow);

        return createLocationResult;
    }
}