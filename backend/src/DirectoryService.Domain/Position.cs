using CSharpFunctionalExtensions;
using DirectoryService.Domain.Abstractions;
using DirectoryService.Domain.DepartmentValueObjects;
using DirectoryService.Domain.Err;
using DirectoryService.Domain.PositionValueObjects;

namespace DirectoryService.Domain;

public sealed class Position : Entity<PositionId>
{
    private readonly List<DepartmentsPositions> _departmentsPositions = [];

    public PositionName Name { get; private set; }

    public Description? Description { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyList<DepartmentsPositions> DepartmentsPositions => _departmentsPositions;

    private Position()
    {
    }

    private Position(
        PositionName name,
        Description? description,
        DateTime createdAt)
    {
        Id = PositionId.Create();
        Name = name;
        Description = description;
        IsActive = true;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public static Result<Position, Error> Create(
        PositionName name,
        Description? description,
        IClock clock)
    {
        var createdAt = clock.UtcNow();

        if (createdAt.Kind is not DateTimeKind.Utc)
        {
            var error = Error.Create(
                $"{nameof(createdAt)} must be UTC.",
                "invalid.parameter",
                ErrorTypes.VALIDATION);
            return error;
        }

        return new Position(name, description, createdAt);
    }

    public void AddDepartments(IEnumerable<DepartmentId> departments)
    {
        var departmentsPositions =
            departments.Select(x => new DepartmentsPositions(x, this.Id));

        _departmentsPositions.AddRange(departmentsPositions);
    }
}