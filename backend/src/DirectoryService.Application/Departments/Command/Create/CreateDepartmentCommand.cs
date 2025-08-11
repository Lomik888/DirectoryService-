using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts.Requests;

namespace DirectoryService.Application.Departments.Command;

public record CreateDepartmentCommand(CreateDepartmentRequest Request) : ICommand
{
    public static implicit operator CreateDepartmentCommand(CreateDepartmentRequest request) => new(request);
};