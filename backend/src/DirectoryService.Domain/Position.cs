using CSharpFunctionalExtensions;
using DirectoryService.Domain.Error;
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

    public static Result<Position, Error.Error> Create(
        PositionName name,
        Description? description,
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

        return new Position(name, description, createdAt);
    }
}