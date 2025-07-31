using CSharpFunctionalExtensions;
using DirectoryService.Domain.DepartmentValueObjects;
using DirectoryService.Domain.Error;
using Path = DirectoryService.Domain.DepartmentValueObjects.Path;

namespace DirectoryService.Domain;

public sealed class Department : Entity<DepartmentId>
{
    private readonly List<Department> _departments = [];
    private readonly List<Location> _locations = [];
    private readonly List<Position> _positions = [];

    public DepartmentName Name { get; private set; }

    public Identifier Identifier { get; private set; }

    public DepartmentId? ParentId { get; private set; }

    public Path Path { get; private set; }

    public short Depth { get; private set; }

    public int ChildrenCount { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyList<Department> ChildrenDepartments => _departments;

    public IReadOnlyList<Location> Locations => _locations;

    public IReadOnlyList<Position> Positions => _positions;

    private Department(
        DepartmentName name,
        Identifier identifier,
        Path path,
        DateTime createdAt)
    {
        Id = DepartmentId.Create();
        Name = name;
        Identifier = identifier;
        IsActive = true;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
        ChildrenCount = ChildrenDepartments.Count;
        Path = path;
        Depth = Path.GetDepth();
    }

    public static Result<Department, Error.Error> Create(
        DepartmentName name,
        Identifier identifier,
        Path path,
        DateTime createdAt)
    {
        if (createdAt.Kind is not DateTimeKind.Utc)
        {
            var error = Error.Error.Create(
                $"{nameof(createdAt)} must be UTC.",
                "invalid.parameter",
                ErrorTypes.VALIDATION);
            return error;
        }

        return new Department(name, identifier, path, createdAt);
    }
}