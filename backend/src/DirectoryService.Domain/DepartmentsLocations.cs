using DirectoryService.Domain.DepartmentValueObjects;
using DirectoryService.Domain.LocationValueObjects;

namespace DirectoryService.Domain;

public class DepartmentsLocations
{
    public Guid Id { get; set; }

    public DepartmentId DepartmentId { get; set; }

    public LocationId LocationId { get; set; }

    private DepartmentsLocations()
    {
    }

    public DepartmentsLocations(DepartmentId departmentId, LocationId locationId)
    {
        Id = Guid.NewGuid();
        DepartmentId = departmentId;
        LocationId = locationId;
    }
}