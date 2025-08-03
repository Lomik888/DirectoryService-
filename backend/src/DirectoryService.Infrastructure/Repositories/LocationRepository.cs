using System.Text.Json;
using DirectoryService.Application.Locations;
using DirectoryService.Domain;
using DirectoryService.Domain.LocationValueObjects;
using Microsoft.EntityFrameworkCore;

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

    public async Task<bool> LocationExistsAsync(
        LocationName locationName,
        Address address,
        CancellationToken cancellationToken)
    {
        string addressJson = address.ToJson();

        var result = await _context.Locations.Where(x =>
                x.Name == locationName ||
                (x.Name == locationName && EF.Functions.JsonContains(x.Addresses, addressJson)))
            .AnyAsync(cancellationToken);

        return result;
    }
}