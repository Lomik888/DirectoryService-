using CSharpFunctionalExtensions;
using DirectoryService.Domain;
using DirectoryService.Domain.DepartmentValueObjects;
using DirectoryService.Domain.Err;
using Path = DirectoryService.Domain.DepartmentValueObjects.Path;

namespace DirectoryService.Application.Departments;

public interface IDepartmentsRepository
{
    Task MinusChildrenCountAsync(DepartmentId departmentId, int count, CancellationToken cancellationToken);

    Task UpdatePathsAndDephtsAsync(Path old, Path @new, CancellationToken cancellationToken);

    Task<Result<Department, Errors>> GetByIdAsync(
        DepartmentId id,
        CancellationToken cancellationToken);

    Task<Result<Department, Errors>> GetByIdWithLocationsAsync(
        DepartmentId id,
        CancellationToken cancellationToken);

    Task<List<DepartmentId>> DepartmentsIsActiveAndExistsAsync(
        IEnumerable<DepartmentId> locationIds,
        CancellationToken cancellationToken);

    Task<bool> DepartmentIsActiveAndExistsAsync(
        DepartmentId id,
        CancellationToken cancellationToken);

    Task AddAsync(Department department, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}