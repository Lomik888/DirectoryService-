using CSharpFunctionalExtensions;
using DirectoryService.Domain.PositionValueObjects;

namespace DirectoryService.Domain;

public sealed class Position : Entity<PositionId>
{
    private readonly List<Department> _departments = [];

    public PositionName Name { get; private set; }

    public Description? Description { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public IReadOnlyList<Department> Departments => _departments;

    public Position(
        PositionName name,
        Description? description)
    {
        Id = PositionId.Create();
        Name = name;
        Description = description;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }
}