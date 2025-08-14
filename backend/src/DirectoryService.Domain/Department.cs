using System.Runtime.InteropServices;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Abstractions;
using DirectoryService.Domain.DepartmentValueObjects;
using DirectoryService.Domain.Err;
using DirectoryService.Domain.LocationValueObjects;
using Path = DirectoryService.Domain.DepartmentValueObjects.Path;

namespace DirectoryService.Domain;

public sealed class Department : Entity<DepartmentId>
{
    private readonly List<Department> _departments = [];
    private List<DepartmentsLocations> _departmentsLocations = [];
    private readonly List<DepartmentsPositions> _departmentsPositions = [];

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

    public IReadOnlyList<DepartmentsLocations> DepartmentsLocations => _departmentsLocations;

    public IReadOnlyList<DepartmentsPositions> DepartmentsPositions => _departmentsPositions;

    private Department()
    {
    }

    private Department(
        DepartmentId departmentId,
        DepartmentName name,
        Identifier identifier,
        DepartmentId? parentId,
        Path path,
        short depth,
        int childrenCount,
        bool isActive,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = departmentId;
        Name = name;
        Identifier = identifier;
        ParentId = parentId;
        Path = path;
        Depth = depth;
        ChildrenCount = childrenCount;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Result<Department, Error> CreateParent(
        DepartmentName name,
        Identifier identifier,
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

        var departmentId = DepartmentId.Create();
        var path = Path.Create($"{identifier.Value}/").Value;
        var depth = (short)0;
        var childrenCount = 0;
        var isActive = true;
        var updatedAt = createdAt;


        return new Department(
            departmentId,
            name,
            identifier,
            null,
            path,
            depth,
            childrenCount,
            isActive,
            createdAt,
            updatedAt);
    }

    public Result<Department, Error> AddChild(
        DepartmentName name,
        Identifier identifier,
        IClock clock)
    {
        var identifierExists = this.Path.ValidateChildPath(identifier);
        if (identifierExists.IsFailure == true)
        {
            return identifierExists.Error;
        }

        var childCreatedAt = clock.UtcNow();

        if (childCreatedAt.Kind is not DateTimeKind.Utc)
        {
            var error = Error.Create(
                $"{nameof(childCreatedAt)} must be UTC.",
                "invalid.parameter",
                ErrorTypes.VALIDATION);
            return error;
        }

        var children = CreateChild(
            name,
            identifier,
            childCreatedAt);

        this.ChildrenCount += 1;
        this._departments.Add(children);
        this.UpdatedAt = childCreatedAt;

        return children;
    }

    public void AddLocations(List<LocationId> locationId)
    {
        var departmentsLocations =
            locationId.Select(x => new DepartmentsLocations(this.Id, x));

        _departmentsLocations.AddRange(departmentsLocations);
    }

    public void AddLocationsWithClear(List<LocationId> locationId)
    {
        var departmentsLocations =
            locationId.Select(x => new DepartmentsLocations(this.Id, x)).ToList();

        this._departmentsLocations = departmentsLocations;
    }

    private Department CreateChild(
        DepartmentName name,
        Identifier identifier,
        DateTime createdAt)
    {
        var departmentId = DepartmentId.Create();
        var parentId = this.Id;
        var path = Path.Create(this.Path, identifier.Value).Value;
        var depth = (short)(1 + this.Depth);
        var childrenCount = 0;
        var isActive = true;
        var updatedAt = createdAt;

        var children = new Department(
            departmentId,
            name,
            identifier,
            parentId,
            path,
            depth,
            childrenCount,
            isActive,
            createdAt,
            updatedAt);

        return children;
    }
}