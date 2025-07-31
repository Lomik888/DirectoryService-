using CSharpFunctionalExtensions;
using DirectoryService.Domain.DepartmentValueObjects;
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

    public Department(
        DepartmentName name,
        Identifier identifier,
        Path path)
    {
        Id = DepartmentId.Create();
        Name = name;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
        ChildrenCount = ChildrenDepartments.Count;
        Path = path;
        Depth = Path.GetDepth();
    }
}