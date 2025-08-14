using CSharpFunctionalExtensions;
using DirectoryService.Application.Departments;
using DirectoryService.Domain;
using DirectoryService.Domain.DepartmentValueObjects;
using DirectoryService.Domain.Err;
using DirectoryService.Domain.Extations;
using Microsoft.EntityFrameworkCore;
using Path = DirectoryService.Domain.DepartmentValueObjects.Path;

namespace DirectoryService.Infrastructure.Repositories;

public class DepartmentsRepository : IDepartmentsRepository
{
    private readonly DirectoryServiceDbContext _context;

    public DepartmentsRepository(DirectoryServiceDbContext context)
    {
        _context = context;
    }

    public async Task MinusChildrenCountAsync(DepartmentId departmentId, int count, CancellationToken cancellationToken)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"""
                update department.departments
                set children_count = children_count - {count}
                where id = {departmentId.Value};
             """, cancellationToken);
    }

    public async Task UpdatePathsAndDephtsAsync(Path old, Path @new, CancellationToken cancellationToken)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"""
                update department.departments
                    set path = {@new.Value} || substring(path FROM char_length({old.Value}) + 1),
                        depth = length({@new.Value} || substring(path FROM char_length({old.Value}) + 1))
                                - length(replace({@new.Value} || substring(path FROM char_length({old.Value}) + 1), '/', ''))
                    where path like {old.Value + "%"};
             """, cancellationToken);
    }

    public async Task<Result<Department, Errors>> GetByIdAsync(
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

    public async Task<Result<Department, Errors>> GetByIdWithLocationsAsync(DepartmentId id,
        CancellationToken cancellationToken)
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