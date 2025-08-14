using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts.Requests;

namespace DirectoryService.Application.Departments.Command.UpdateLocations;

public record UpdateDepartmentsLocationsCommand(
    Guid DepartmentId,
    UpdateDepartmentsLocationsRequest Request) : ICommand;