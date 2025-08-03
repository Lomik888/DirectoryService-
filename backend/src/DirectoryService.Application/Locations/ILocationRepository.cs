using DirectoryService.Domain;
using DirectoryService.Domain.LocationValueObjects;

namespace DirectoryService.Application.Locations;

public interface ILocationRepository
{
    Task AddAsync(Location location, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);

    Task<bool> LocationExistsAsync(LocationName locationName, Address address, CancellationToken cancellationToken);
}