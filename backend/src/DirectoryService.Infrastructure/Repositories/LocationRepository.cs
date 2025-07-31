using DirectoryService.Application.Locations;
using DirectoryService.Domain;

namespace DirectoryService.Infrastructure.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly DirectoryServiceDbContext _context;

    public LocationRepository(DirectoryServiceDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Location location, CancellationToken cancellationToken)
    {
        await _context.AddAsync(location, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}