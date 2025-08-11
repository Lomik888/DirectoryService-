using CSharpFunctionalExtensions;
using DirectoryService.Application.Departments;
using DirectoryService.Domain;
using DirectoryService.Domain.DepartmentValueObjects;
using DirectoryService.Domain.Err;
using DirectoryService.Domain.Extations;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Infrastructure.Repositories;

public class DepartmentsRepository : IDepartmentsRepository
{
    private readonly DirectoryServiceDbContext _context;

    public DepartmentsRepository(DirectoryServiceDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Department, Errors>> GetByIdWithoutNavidationsProperties(
        DepartmentId id,
        CancellationToken cancellationToken)
    {
        var department = await _context.Departments
            .Where(x => x.Id == id && x.IsActive)
            .SingleOrDefaultAsync(cancellationToken);
        if (department == null)
        {
            var error = GeneralErrors.Validation.NotFound(
                $"Department with id: {id.Value} was not found",
                "not.found");
            return error.ToErrors();
        }

        return department;
    }

    public async Task<Result<Department, Errors>> GetByIdAsync(DepartmentId id, CancellationToken cancellationToken)
    {
        var department = await _context.Departments
            .Where(x => x.Id == id && x.IsActive)
            .Include(x => x.DepartmentsLocations)
            .SingleOrDefaultAsync(cancellationToken);
        if (department == null)
        {
            var error = GeneralErrors.Validation.NotFound(
                $"Department with id: {id.Value} was not found",
                "not.found");
            return error.ToErrors();
        }

        return department;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Вернёт те, которые существуют.</returns>
    public async Task<List<DepartmentId>> DepartmentsIsActiveAndExistsAsync(
        IEnumerable<DepartmentId> ids,
        CancellationToken cancellationToken)
    {
        var departmentExists = await _context.Departments
            .Where(x => (ids.Contains(x.Id) == true && x.IsActive == true) == true)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        return departmentExists;
    }

    public async Task<bool> DepartmentIsActiveAndExistsAsync(
        DepartmentId id,
        CancellationToken cancellationToken)
    {
        var departmentExists = await _context.Departments
            .Where(x => id == x.Id && x.IsActive == true)
            .AnyAsync(cancellationToken);

        return departmentExists;
    }

    public async Task AddAsync(Department department, CancellationToken cancellationToken)
    {
        await _context.AddAsync(department, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}