using DirectoryService.Domain;

namespace DirectoryService.Application.Locations;

public interface ILocationRepository
{
    Task AddAsync(Location location, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}