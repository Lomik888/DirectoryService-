using DirectoryService.Application.Positions;
using DirectoryService.Domain;
using DirectoryService.Domain.PositionValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Infrastructure.Repositories;

public class PositionRepository : IPositionRepository
{
    private readonly DirectoryServiceDbContext _context;

    public PositionRepository(DirectoryServiceDbContext context)
    {
        _context = context;
    }

    public async Task<bool> NameIsUniqueAsync(PositionName name, CancellationToken cancellationToken)
    {
        var positionExists = await _context.Positions
            .Where(x => x.Name == name && x.IsActive == true)
            .AnyAsync(cancellationToken);

        return !positionExists;
    }

    public async Task AddAsync(Position position, CancellationToken cancellationToken)
    {
        await _context.AddAsync(position, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}