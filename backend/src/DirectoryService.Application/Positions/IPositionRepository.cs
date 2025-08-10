using DirectoryService.Domain;
using DirectoryService.Domain.PositionValueObjects;

namespace DirectoryService.Application.Positions;

public interface IPositionRepository
{
    Task<bool> NameIsUniqueAsync(PositionName name, CancellationToken cancellationToken);

    Task AddAsync(Position position, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}