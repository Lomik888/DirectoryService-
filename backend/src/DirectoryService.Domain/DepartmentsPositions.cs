using DirectoryService.Domain.DepartmentValueObjects;
using DirectoryService.Domain.PositionValueObjects;

namespace DirectoryService.Domain;

public class DepartmentsPositions
{
    public Guid Id { get; set; }

    public DepartmentId DepartmentId { get; set; }

    public PositionId PositionId { get; set; }

    private DepartmentsPositions()
    {
    }

    public DepartmentsPositions(DepartmentId departmentId, PositionId positionId)
    {
        Id = Guid.NewGuid();
        DepartmentId = departmentId;
        PositionId = positionId;
    }
}